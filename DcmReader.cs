using EvilDICOM.Core;
using EvilDICOM.Core.Image;
using EvilDICOM.Core.Selection;

namespace MPPG
{
    internal class DcmReader
    {
        public struct CalculatedData
        {
            public string? Manufacturer { get; internal set; }

            /*
             * Offset value represents the offset from the dicom origin to the users chosen 
             * isocenter in the plane for the given beam, or other way around
             */
            public Float3Struct? Offset { get; internal set; }
            public float[] X { get; internal set; }
            public float[] Y { get; internal set; }
            public float[] Z { get; internal set; }
            public float[,,] V { get; internal set; }
        }

        public static CalculatedData Read(DICOMSelector dcmSel, PixelStream pixelStream, string filePath)
        {
            var data = new CalculatedData
            {
                Manufacturer = dcmSel.Manufacturer.Data,
                Offset = FindOffset(dcmSel, filePath)
            };

            if (data.Offset == null)
            {
                // TODO: Ask user for offset
            }

            ReadData(dcmSel, pixelStream, ref data);

            return data;
        }

        /**
         * Accepts DICOM DOSE file selector and the main file location and determines
         * if the associated DICOM-RT PLAN and RT STRUCT are in the same
         * directory. If so, the DICOM offset for use in MPPG_GUI is determined.
         * A structure is returned. At this time, only the DICOM offset is sent
         * back, but this could be used to send addtional plan information.
         */
        private static Float3Struct? FindOffset(DICOMSelector dcmSel, string filePath)
        {
            var planSeq = dcmSel.ReferencedRTPlanSequence.Data;
            if (planSeq == null)
                return null;

            var planSeqSel = planSeq.GetSelector();
            var refSOPInstId = planSeqSel.ReferencedSOPInstanceUID.Data;

            var folder = Path.GetDirectoryName(filePath);
            var files = Directory.EnumerateFiles(folder, "*.dcm");
            foreach (var planFileName in files)
            {
                // Search for matching DICOM-RT PLAN file. Skip the main DCM file.
                if (planFileName.Equals(filePath))
                    continue;

                var planFile = DICOMObject.Read(planFileName);
                var planFileSel = planFile.GetSelector();
                if (planFileSel.Modality.Data != "RTPLAN")
                    continue; // Not a DICOM-RT PLAN file

                var instId = planFileSel.SOPInstanceUID.Data;
                if (instId == null)
                    continue;

                if (instId.Equals(refSOPInstId))
                {
                    // Matching plan file has been found
                    // The next step is trying to find the DICOM offset. The first place we
                    // will look is in the DICOM-RT PLAN.
                    if (planFileSel.DoseReferenceSequence != null)
                    {
                        var sequences = planFileSel.DoseReferenceSequence.Data_;
                        // TODO:Search over all POIs in the Plan Dose Reference Sequence
                    }
                    else
                    {
                        // The seach of DoseReferenceSequence did not turn up a point called "ORIGIN".
                        // Let's continue with a search for a structure set.
                        // Get Reference RT Plan Sequence
                        var refStructSeq = planFileSel.ReferencedStructureSetSequence.Data;
                        if (refStructSeq == null)
                            continue;

                        var refStructSeqSel = refStructSeq.GetSelector();
                        var refSeqSOPInstId = refStructSeqSel.ReferencedSOPInstanceUID.Data;

                        // Cycle through the DICOM-RT STRUCT files and check to see if the ReferencedStructureSetSequence ID
                        // from the PLAN file matches the SOP Instance UID in the structure set file.
                        // If it does, you have a pair.
                        foreach (var structFileName in files)
                        {
                            var structFile = DICOMObject.Read(structFileName);
                            var structFileSel = structFile.GetSelector();

                            if (structFileSel.Modality.Data != "RTSTRUCT")
                                continue; // Not a DICOM-RT STRUCT file

                            var structInstId = structFileSel.SOPInstanceUID.Data;
                            if (structInstId == null || !structInstId.Equals(refSeqSOPInstId))
                                continue;

                            // Matching structure file has been found
                            // We've reached the end of our search. The last step is searching the
                            // structure set for a POI called ORIGIN.If none is found, we'll ask
                            // the user for an offset.
                            var structSeq = structFileSel.StructureSetROISequence.Data_;
                            foreach (var strObject in structSeq)
                            {
                                var strObjectSel = strObject.GetSelector();
                                if (strObjectSel.ROIName.Data == "ORIGIN")
                                {
                                    // Now we take ROI number
                                    var roiNum = strObjectSel.ROINumber.Data;

                                    // Search the ROIContourSequence for an ROI with the same ReferencedROINumber
                                    var contourSeqs = structFileSel.ROIContourSequence.Data_;
                                    foreach(var contourSeqObj in contourSeqs)
                                    {
                                        var contourSeqObjSel = contourSeqObj.GetSelector();
                                        var refROINum = contourSeqObjSel.ReferencedROINumber.Data;
                                        if (refROINum == roiNum)
                                        {
                                            var contSeq = contourSeqObjSel.ContourSequence.Data;
                                            var contSeqSel = contSeq.GetSelector();
                                            var data = contSeqSel.ContourData.Data_;
                                            return new Float3Struct((float)(data[0]/10), (float)data[1]/10, (float)data[2]/10);
                                        }
                                    }
                                }
                            }
                            break;
                        }
                    }
                    break;
                }
            }
            return null;
        }

        private static bool ReadData(DICOMSelector dcmSel, PixelStream pixelStream, ref CalculatedData calcData)
        {
            // Elements in X
            var cols = dcmSel.Columns.Data;

            // Elements in Y
            var rows = dcmSel.Rows.Data;

            // Elements in Z
            var deps = dcmSel.NumberOfFrames.Data;

            var imagePositionPatient = dcmSel.ImagePositionPatient.Data_;
            var pixelSpacing = dcmSel.PixelSpacing.Data_;
            var offset = calcData.Offset.Value;

            // Establish Coordinate System[in cm]
            // Note: PixelSpacing indices do not match ImagePositionPatient indices.
            calcData.X = new float[cols];
            for(int i = 0; i < cols; i++)
                calcData.X[i] = (float)(imagePositionPatient[0] + pixelSpacing[1] * i) / 10 - offset.X;

            calcData.Y = new float[rows];
            for (int i = 0; i < rows; i++)
                calcData.Y[i] = (float)(imagePositionPatient[1] + pixelSpacing[0] * i) / 10 - offset.Y;

            var data = dcmSel.GridFrameOffsetVector.Data_;
            calcData.Z = new float[data.Count];
            for (int i = 0; i < data.Count; i++)
                calcData.Z[i] = (float)(imagePositionPatient[2] + data[i]) / 10 - offset.Z;

            //var sr = new BinaryReader(pixelStream);
            var buf = new byte[pixelStream.Length];
            pixelStream.Read(buf, 0, buf.Length);
            pixelStream.Close();

            // TODO: Can data be stored as anything else?
            // var bits = dcmSel.BitsStored.Data;

            var scale = (float)dcmSel.DoseGridScaling.Data;

            if (dcmSel.DoseUnits.Data == "CGY")
                scale /= 100;

            int maxX = 0, maxY = 0, maxZ = 0;
            float maxV = 0f;
            calcData.V = new float[calcData.X.Length, calcData.Y.Length, calcData.Z.Length];
            for (int z = 0; z < calcData.Z.Length; z++)
                for (int y = 0; y < calcData.Y.Length; y++)
                    for (int x = 0; x < calcData.X.Length; x++)
                    {
                        //var v = sr.ReadUInt16();
                        var index = (x + y * calcData.X.Length + z * calcData.X.Length * calcData.Y.Length) * 2;
                        var v = (ushort)((buf[index]) | (buf[index + 1]) << 8);
                        calcData.V[x, y, z] = v * scale;
                        if (maxV < v * scale)
                        {
                            maxV = (float)(v * scale);
                            maxX = x;
                            maxY = y;
                            maxZ = z;
                        }
                    }

            return true;
        }
    }
}
