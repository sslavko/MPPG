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
            public Float3Struct? Offset { get; internal set; }
            public List<Float4Struct>? Data { get; internal set; }
        }

        public static CalculatedData Read(DICOMSelector dcmSel, PixelStream pixelStream, string filePath)
        {
            var ret = new CalculatedData();
            ret.Manufacturer = dcmSel.Manufacturer.Data;
            ret.Offset = FindOffset(dcmSel, filePath);

            if (ret.Offset == null)
            {
                // TODO: Ask user for offset
            }

            ReadData(dcmSel, pixelStream, ret);

            return ret;
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

        private static bool ReadData(DICOMSelector dcmSel, PixelStream pixelStream, CalculatedData calcData)
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
            List<double> x = [];
            for(int i = 0; i < cols; i++)
                x.Add((imagePositionPatient[0] + pixelSpacing[1] * i) / 10 - offset.X);

            List<double> y = [];
            for (int i = 0; i < rows; i++)
                y.Add((imagePositionPatient[1] + pixelSpacing[0] * i) / 10 - offset.Y);

            List<double> z = [];
            var data = dcmSel.GridFrameOffsetVector.Data_;
            for (int i = 0; i < data.Count; i++)
                z.Add((imagePositionPatient[2] + data[i]) / 10 - offset.Z);

            var sr = new BinaryReader(pixelStream);
            var bits = dcmSel.BitsStored.Data;
            var p = sr.ReadUInt16();

            var scale = dcmSel.DoseGridScaling.Data;

            /*if (dcmSel.DoseUnits.Data == "CGY")
                dose = dose / 100;*/

            return false;
        }
    }
}
