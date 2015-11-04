﻿// Copyright (c) 2012-2015 fo-dicom contributors.
// Licensed under the Microsoft Public License (MS-PL).

namespace Dicom
{
    using System;

    using Dicom.IO.Buffer;

    using Xunit;

    [Collection("General")]
    public class DicomElementTest
    {
        #region Unit tests

        [Fact]
        public void DicomSignedShort_Array_GetDefaultValue()
        {
            DicomSignedShort element = new DicomSignedShort(DicomTag.SynchronizationChannel, 5, 8);
            Assert.Equal((short)5, element.Get<short>());
        }

        [Fact]
        public void DicomSignedShortAsDicomElement_Array_GetDefaultValue()
        {
            DicomElement element = new DicomSignedShort(DicomTag.SynchronizationChannel, 5, 8);
            Assert.Equal((short)5, element.Get<short>());
        }

        [Fact]
        public void AttributeTagAsDicomElement_Array_GetDefaultValue()
        {
            var expected = DicomTag.ALinePixelSpacing;
            DicomElement element = new DicomAttributeTag(DicomTag.DimensionIndexPointer, DicomTag.ALinePixelSpacing);
            var actual = element.Get<DicomTag>();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DicomUnsignedShort_Array_ExplicitMinus1InterpretAs0()
        {
            var element = new DicomUnsignedShort(DicomTag.ReferencedFrameNumbers, 1, 2, 3, 4, 5);
            Assert.Equal(element.Get<ushort>(-1), element.Get<ushort>(0));
        }

        [Fact]
        public void DicomUnsignedShort_Array_ExplicitMinus2Throws()
        {
            var element = new DicomUnsignedShort(DicomTag.ReferencedFrameNumbers, 1, 2, 3, 4, 5);
            Assert.Throws<ArgumentOutOfRangeException>(() => element.Get<ushort>(-2));
        }

        [Fact]
        public void DicomOtherDouble_ByteBuffer_ReturnsValidNumber()
        {
            var element = new DicomOtherDouble(DicomTag.DoubleFloatPixelData, new MemoryByteBuffer(new byte[] { 0x10, 0x20, 0x30, 0x40, 0x50, 0x60, 0x70, 0x80 }));
            var actual = element.Get<double>();
            Assert.InRange(actual, double.MinValue, double.MaxValue);
        }

        [Fact]
        public void DicomUnlimitedCharacters_MultipleStrings_ReturnsDelimitedString()
        {
            var element = new DicomUnlimitedCharacters(DicomTag.DoubleFloatPixelData, "a", "b", "c");
            var actual = element.Get<string>();
            Assert.Equal(@"a\b\c", actual);
        }

        [Fact]
        public void DicomPersonName_FamilyAndSurname_YieldsCompositeName()
        {
            var element = new DicomPersonName(DicomTag.ConsultingPhysicianName, "Doe", "John");
            var actual = element.Get<string>(0);
            Assert.Equal("Doe^John", actual);
        }

        [Fact]
        public void DicomPersonName_TwoNames_YieldsTwoValues()
        {
            var element = new DicomPersonName(DicomTag.ConsultingPhysicianName, new [] { "Doe^John", "Bar^Foo"});
            var actual = element.Get<string[]>();
            Assert.Equal(2, actual.Length);
            Assert.Equal("Bar^Foo", actual[1]);
        }

        #endregion
    }
}