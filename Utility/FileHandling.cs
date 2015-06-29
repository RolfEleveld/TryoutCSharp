using System.IO;

namespace Utility
{
    public static class FileHandling
    {
        /// <summary>
        /// Reads the text file write texts containing to new file using streams.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="parseString">The parse string.</param>
        /// <returns></returns>
        public static string ReadTextFileWriteTextsContainingToNewFileUsingStreams( string filePath, string parseString )
        {
            string resultFileName = null;
            if ( File.Exists( filePath ) )
            {
                resultFileName = Path.GetTempFileName();
                using ( TextReader reader = new StreamReader( filePath ) )
                {
                    using ( TextWriter writer = new StreamWriter( resultFileName ) )
                    {
                        string line;
                        if ( ( line = reader.ReadLine() ).Contains( parseString ) )
                        {
                            writer.WriteLine( line );
                        }
                    }
                }
            }
            return resultFileName;

        }
        /// <summary>
        /// Readtexts the file write text containing to new file using file text.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="parseString">The parse string.</param>
        /// <returns></returns>
        public static string ReadtextFileWriteTextContainingToNewFileUsingFileText( string filePath, string parseString )
        {
            string resultFileName = null;
            if ( File.Exists( filePath ) )
            {
                resultFileName = Path.GetTempFileName();
                using ( TextReader reader = File.OpenText( filePath ) )
                {
                    using ( TextWriter writer = File.CreateText( resultFileName ) )
                    {
                        string line;
                        if ( ( line = reader.ReadLine() ).Contains( parseString ) )
                        {
                            writer.WriteLine( line );
                        }
                    }
                }
            }
            return resultFileName;
        }

        /// <summary>
        /// Writes the object out to binary file, only i.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string WriteOutObjectToBinaryFile( object value )
        {
            string resultFileName = null;
            if ( value != null)
            {
                resultFileName = Path.GetTempFileName();
                using (BinaryWriter writer = new BinaryWriter(File.Open(resultFileName, FileMode.OpenOrCreate, FileAccess.Write)))
                {
                    switch ((value.GetType().Name))
                    {
                        case "bool":
                            writer.Write((bool) value );
                            break;
                        case "byte":
                            writer.Write((byte) value );
                            break;
                        case "byte[]":
                            writer.Write((byte[]) value );
                            break;
                        case "char":
                            writer.Write((char) value );
                            break;
                        case "char[]":
                            writer.Write((char[]) value );
                            break;
                        case "decimal":
                            writer.Write((decimal) value );
                            break;
                        case "double":
                            writer.Write((double) value );
                            break;
                        case "float":
                            writer.Write((float) value );
                            break;
                        case "int":
                            writer.Write((int) value );
                            break;
                        case "long":
                            writer.Write((long) value );
                            break;
                        case "sbyte":
                            writer.Write((sbyte) value );
                            break;
                        case "short":
                            writer.Write((short) value );
                            break;
                        case "string":
                            writer.Write((string) value );
                            break;
                        case "uint":
                            writer.Write((uint) value );
                            break;
                        case "ulong":
                            writer.Write((ulong) value );
                            break;
                        case "ushort":
                            writer.Write((ushort) value );
                            break;
                    }
                }
            }
            return resultFileName;
        }


    }
}