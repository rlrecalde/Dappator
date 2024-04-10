using System;

namespace Dappator.MySql.Test.Model
{
    [System.ComponentModel.Description("`DataType`")]
    public class DataTypeNullable
    {
        [System.ComponentModel.Description("`Id`")]
        public int Id { get; set; }

        [System.ComponentModel.Description("`Byte`")]
        public byte? Byte { get; set; }

        [System.ComponentModel.Description("`Sbyte`")]
        public sbyte? Sbyte { get; set; }

        [System.ComponentModel.Description("`Short`")]
        public short? Short { get; set; }

        [System.ComponentModel.Description("`Ushort`")]
        public ushort? Ushort { get; set; }

        [System.ComponentModel.Description("`Int`")]
        public int? Int { get; set; }

        [System.ComponentModel.Description("`Uint`")]
        public uint? Uint { get; set; }

        [System.ComponentModel.Description("`Long`")]
        public long? Long { get; set; }

        [System.ComponentModel.Description("`Ulong`")]
        public ulong? Ulong { get; set; }

        [System.ComponentModel.Description("`Float`")]
        public float? Float { get; set; }

        [System.ComponentModel.Description("`Double`")]
        public double? Double { get; set; }

        [System.ComponentModel.Description("`Decimal`")]
        public decimal? Decimal { get; set; }

        [System.ComponentModel.Description("`Currency`")]
        public float? Currency { get; set; }

        [System.ComponentModel.Description("`Bool`")]
        public bool? Bool { get; set; }

        [System.ComponentModel.Description("`String`")]
        public string String { get; set; }

        [System.ComponentModel.Description("`Char`")]
        public char? Char { get; set; }

        [System.ComponentModel.Description("`Guid`")]
        public string Guid { get; set; }

        [System.ComponentModel.Description("`DateTime`")]
        public DateTime? DateTime { get; set; }

        [System.ComponentModel.Description("`DateTimeOffset`")]
        public DateTimeOffset? DateTimeOffset { get; set; }

        [System.ComponentModel.Description("`TimeSpan`")]
        public TimeSpan? TimeSpan { get; set; }

        [System.ComponentModel.Description("`Bytes`")]
        public byte[] Bytes { get; set; }
    }
}
