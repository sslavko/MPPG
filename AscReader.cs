namespace MPPG
{
    internal class AscReader
    {
        public struct FieldSizeStruct(int x, int y)
        {
            public int X { get; internal set; } = x;
            public int Y { get; internal set; } = y;
        }

        public struct BeamData(int n)
        {
            public List<float> X { get; internal set; } = new(n);
            public List<float> Y { get; internal set; } = new(n);
            public List<float> Z { get; internal set; } = new(n);
            public List<float> V { get; internal set; } = new(n);
        }

        public struct MeasurementStruct
        {
            public string? Version { get; internal set; }
            public string? Date { get; internal set; }
            public string? Time { get; internal set; }
            public string? BeamType { get; internal set; }
            public float BeamEnergy { get; internal set; }
            public FieldSizeStruct FieldSize { get; internal set; }
            public string? DataType { get; internal set; }
            public int NumPoints { get; internal set; }
            public int SSD { get; internal set; }
            public Float3Struct StartPos { get; internal set; }
            public Float3Struct EndPos { get; internal set; }
            public BeamData BeamData { get; internal set; }
            public char AxisType { get; internal set; }
            public float Depth { get; internal set; }
        }

        public struct MeasurementData
        {
            public int NumberOfMeasurements { get; internal set; }
            public string? ScannerSystem { get; internal set; }
            public List<MeasurementStruct>? Data;
        }

        public static MeasurementData? Read(string filePath) 
        {  
            if (!File.Exists(filePath)) 
                return null;

            MeasurementData ret;

            using (var reader = new StreamReader(filePath))
            {
                var line = reader.ReadLine();

                // The first line must have number of measurements
                if (line == null || !line.StartsWith(":MSR"))
                    return null;

                ret = new MeasurementData();
                var index = line.IndexOf('#');
                var val = index == -1 ? line[4..] : line[4..index];
                ret.NumberOfMeasurements = int.Parse(val.Trim());

                line = reader.ReadLine();

                // Second line is beam data scanner system
                if (line == null || !line.StartsWith(":SYS"))
                    return null;

                index = line.IndexOf('#');
                val = index == -1 ? line[4..] : line[4..index];
                ret.ScannerSystem = val.Trim();

                ret.Data = new(ret.NumberOfMeasurements);
                line = reader.ReadLine();
                for (int i = 0; i < ret.NumberOfMeasurements; i++)
                {
                    var measurement = new MeasurementStruct();
                    bool reading = true;
                    float minX = float.PositiveInfinity, maxX = float.NegativeInfinity,
                          minY = float.PositiveInfinity, maxY = float.NegativeInfinity,
                          minZ = float.PositiveInfinity, maxZ = float.NegativeInfinity;

                    while (reading && line != null)
                    {
                        if (line.StartsWith('#'))
                        {   // Skip comment lines
                            line = reader.ReadLine();
                            continue;
                        }

                        index = line.IndexOf('#');
                        if (index > -1)
                        {
                            // Strip comments
                            line = line[..index];
                        }

                        switch (line[..4])
                        {
                            case "%VNR": // Version
                                measurement.Version = line[4..].Trim();
                                break;
                            case "%DAT": // Date
                                measurement.Date = line[4..].Trim();
                                break;
                            case "%TIM": // Time
                                measurement.Time = line[4..].Trim();
                                break;
                            case "%BMT": // Beam type and energy
                                var lineParts = line.Split('\t', StringSplitOptions.TrimEntries);
                                measurement.BeamType = lineParts[1];
                                measurement.BeamEnergy = float.Parse(lineParts[2]);
                                break;
                            case "%FSZ": // Field size (x, y)
                                lineParts = line.Split('\t', StringSplitOptions.TrimEntries);
                                measurement.FieldSize = new FieldSizeStruct(int.Parse(lineParts[1]), int.Parse(lineParts[2]));
                                break;
                            case "%SCN": // Data type
                                measurement.DataType = line[4..].Trim();
                                break;
                            case "%PTS": // Number of points
                                measurement.NumPoints = int.Parse(line[4..].Trim());
                                measurement.BeamData = new BeamData(measurement.NumPoints);
                                break;
                            case "%SSD":
                                measurement.SSD = int.Parse(line[4..].Trim());
                                break;
                            case "%STS": // Start scan values in mm (X, Y, Z)
                                lineParts = line.Split('\t', StringSplitOptions.TrimEntries);
                                measurement.StartPos = new Float3Struct(
                                    float.Parse(lineParts[1]),
                                    float.Parse(lineParts[2]),
                                    float.Parse(lineParts[3]));
                                break;
                            case "%EDS": // End scan values in mm (X, Y, Z)
                                lineParts = line.Split('\t', StringSplitOptions.TrimEntries);
                                measurement.EndPos = new Float3Struct(
                                    float.Parse(lineParts[1]),
                                    float.Parse(lineParts[2]),
                                    float.Parse(lineParts[3]));
                                break;
                            case ":EOM":
                                reading = false;
                                break;
                        }

                        if (line.StartsWith('='))
                        {
                            var lineParts = line.Split('\t', StringSplitOptions.TrimEntries);
                            var x = float.Parse(lineParts[2]) / 10; // cm
                            var y = -float.Parse(lineParts[1]) / 10; // cm
                            var z = float.Parse(lineParts[3]) / 10; // cm
                            var v = float.Parse(lineParts[4]);

                            // Track minimum and maximum values
                            if (x < minX) minX = x;
                            if (y < minY) minY = y;
                            if (z < minZ) minZ = z;
                            if (x > maxX) maxX = x;
                            if (y > maxY) maxY = y;
                            if (z > maxZ) maxZ = z;

                            measurement.BeamData.X.Add(x);
                            measurement.BeamData.Y.Add(y);
                            measurement.BeamData.Z.Add(z);
                            measurement.BeamData.V.Add(v);
                        }

                        line = reader.ReadLine();
                    }
                    if (maxX - minX > .1) measurement.AxisType = 'X';
                    if (maxY - minY > .1) measurement.AxisType = 'Y';
                    if (maxZ - minZ > .1) measurement.AxisType = 'Z';
                    if (measurement.AxisType == 'X' || measurement.AxisType == 'Y')
                        measurement.Depth = minZ;

                    ret.Data.Add(measurement);
                }
                
                reader.Close();
            }
            return ret; 
        }
    }
}
