using System;

namespace Dappator.PostgreSql.Test.Model
{
    [System.ComponentModel.Description("\"datatype\"")]
    public class DataType
    {
        [System.ComponentModel.Description("\"id\"")]
        public int Id { get; set; }

        [System.ComponentModel.Description("\"byte\"")]
        public byte Byte { get; set; }

        [System.ComponentModel.Description("\"sbyte\"")]
        public sbyte Sbyte { get; set; }

        [System.ComponentModel.Description("\"short\"")]
        public short Short { get; set; }

        [System.ComponentModel.Description("\"ushort\"")]
        public ushort Ushort { get; set; }

        [System.ComponentModel.Description("\"int\"")]
        public int Int { get; set; }

        [System.ComponentModel.Description("\"uint\"")]
        public uint Uint { get; set; }

        [System.ComponentModel.Description("\"long\"")]
        public long Long { get; set; }

        [System.ComponentModel.Description("\"ulong\"")]
        public ulong Ulong { get; set; }

        [System.ComponentModel.Description("\"float\"")]
        public float Float { get; set; }

        [System.ComponentModel.Description("\"double\"")]
        public double Double { get; set; }

        [System.ComponentModel.Description("\"decimal\"")]
        public decimal Decimal { get; set; }

        [System.ComponentModel.Description("\"currency\"")]
        public decimal Currency { get; set; }

        [System.ComponentModel.Description("\"bool\"")]
        public bool Bool { get; set; }

        [System.ComponentModel.Description("\"string\"")]
        public string String { get; set; }

        [System.ComponentModel.Description("\"char\"")]
        public char Char { get; set; }

        [System.ComponentModel.Description("\"guid\"")]
        public string Guid { get; set; }

        [System.ComponentModel.Description("\"dateTime\"")]
        public DateTime DateTime { get; set; }

        [System.ComponentModel.Description("\"dateTimeOffset\"")]
        public DateTimeOffset DateTimeOffset { get; set; }

        [System.ComponentModel.Description("\"timeSpan\"")]
        public TimeSpan TimeSpan { get; set; }

        [System.ComponentModel.Description("\"bytes\"")]
        public byte[] Bytes { get; set; }
    }
}
