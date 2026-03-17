using SB_Domain.ValueObjects;

namespace SB_Domain.Tests
{
    public class SB_ValueObjectTests
    {
        // SB_VehicleCode tests

        [Fact]
        public void VehicleCode_WithValidString_CreatesSuccessfully()
        {
            var code = new SB_VehicleCode("CAR-001");
            Assert.Equal("CAR-001", code.Value);
        }

        [Fact]
        public void VehicleCode_WithEmptyString_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new SB_VehicleCode(""));
        }

        [Fact]
        public void VehicleCode_WithWhitespace_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new SB_VehicleCode("   "));
        }

        [Fact]
        public void VehicleCode_WithNull_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new SB_VehicleCode(null!));
        }

        [Fact]
        public void VehicleCode_SameValue_AreEqual()
        {
            var code1 = new SB_VehicleCode("CAR-001");
            var code2 = new SB_VehicleCode("CAR-001");
            Assert.Equal(code1, code2);
        }

        [Fact]
        public void VehicleCode_DifferentValue_AreNotEqual()
        {
            var code1 = new SB_VehicleCode("CAR-001");
            var code2 = new SB_VehicleCode("CAR-002");
            Assert.NotEqual(code1, code2);
        }

        [Fact]
        public void VehicleCode_ToString_ReturnsValue()
        {
            var code = new SB_VehicleCode("CAR-001");
            Assert.Equal("CAR-001", code.ToString());
        }

        // SB_VehicleType tests

        [Fact]
        public void VehicleType_WithValidString_CreatesSuccessfully()
        {
            var type = new SB_VehicleType("Sedan");
            Assert.Equal("Sedan", type.Value);
        }

        [Fact]
        public void VehicleType_WithEmptyString_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new SB_VehicleType(""));
        }

        [Fact]
        public void VehicleType_WithNull_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new SB_VehicleType(null!));
        }

        [Fact]
        public void VehicleType_SameValue_AreEqual()
        {
            var type1 = new SB_VehicleType("SUV");
            var type2 = new SB_VehicleType("SUV");
            Assert.Equal(type1, type2);
        }

        [Fact]
        public void VehicleType_DifferentValue_AreNotEqual()
        {
            var type1 = new SB_VehicleType("Sedan");
            var type2 = new SB_VehicleType("SUV");
            Assert.NotEqual(type1, type2);
        }
    }
}
