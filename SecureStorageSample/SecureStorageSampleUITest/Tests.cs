////////////////////////////////////////////////////////
// Copyright (c) 2017 Sameer Khandekar                //
// License: MIT License.                              //
////////////////////////////////////////////////////////

using System;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace SecureStorageSampleUITest
{
    /// <summary>
    /// Class for testing
    /// </summary>
    [TestFixture(Platform.Android)]
    // [TestFixture(Platform.iOS)] Make changes to AppInitializer for iOS and then Uncomment this
    public class Tests
    {
        IApp app;
        Platform platform;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="platform"></param>
        public Tests(Platform platform)
        {
            this.platform = platform;
        }

        /// <summary>
        /// Start App prior to each test
        /// </summary>
        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
        }

        /// <summary>
        /// Test HasValue for non-existing value
        /// </summary>
        [Test]
        public void HasValue_WithNonExistingKey_ReturnsFalse()
        {
            // Arrange - just type the key, do not add
            app.EnterText("entryKey", "ABC");

            // Act - Tap Has button
            app.Tap("btnHasCommand");

            // Assert
            var result = app.Query(c => c.Marked("lblHasCommand").Text("N"));
            Assert.IsTrue(result.Any(), "HasValue_WithNonExistingKey Failed");
        }

        /// <summary>
        /// Test HasValue for an existing value shows Y
        /// </summary>
        [Test]
        public void HasValue_WithExistingKey_ReturnsTrue()
        {
            // Arrange - store a value with key
            app.EnterText("entryKey", "ABC");
            app.EnterText("entryValue", "XYZ");
            app.Tap("btnSetValue");

            // Act - Tap the button
            app.Tap("btnHasCommand");

            // Assert - should show Y
            var result = app.Query(c => c.Marked("lblHasCommand").Text("Y"));
            Assert.IsTrue(result.Any(), "HasValue_WithExistingKey Failed");
        }

        /// <summary>
        /// Attempt to get value for non-existing key returns default
        /// </summary>
        [Test]
        public void GetValue_WithNonExistingKey_ReturnsDefault()
        {
            // Arrange - just type the key, do not add
            app.EnterText("entryKey", "ABC");

            // Act - Tap Get button
            app.Tap("btnGetValue");

            // Assert
            var result = app.Query(c => c.Marked("lblGetValue").Text(""));
            Assert.IsTrue(result.Any(), "GetValue_WithNonExistingKey Failed");
        }

        /// <summary>
        /// Get value for existing key returns stored value
        /// </summary>
        [Test]
        public void GetValue_WithExistingKey_ReturnsStored()
        {
            // Arrange - store a value with key
            app.EnterText("entryKey", "ABC");
            app.EnterText("entryValue", "XYZ");
            app.Tap("btnSetValue");

            // Act - Tap the button
            app.Tap("btnGetValue");

            // Assert - should show XYZ
            var result = app.Query(c => c.Marked("lblGetValue").Text("XYZ"));
            Assert.IsTrue(result.Any(), "GetValue_WithExistingKey Failed");
        }

        /// <summary>
        /// Attempt to delete non-existing key returns false
        /// </summary>
        [Test]
        public void DeleteKey_WithNonExistingKey_ReturnsFalse()
        {
            // Arrange - just type the key, do not add
            app.EnterText("entryKey", "ABC");

            // Act - Tap Delete button
            app.Tap("btnDelCommand");

            // Assert
            var result = app.Query(c => c.Marked("lblErrMessage").Text("False"));
            Assert.IsTrue(result.Any(), "DeleteKey_WithNonExistingKey Failed");
        }

        /// <summary>
        /// Attempt to delete existing key returns true
        /// </summary>
        [Test]
        public void DeleteKey_WithExistingKey_ReturnsTrue()
        {
            // Arrange - store a value with key
            app.EnterText("entryKey", "ABC");
            app.EnterText("entryValue", "XYZ");
            app.Tap("btnSetValue");

            // Act - Tap the delete button
            app.Tap("btnDelCommand");

            // Assert - should show True
            app.WaitForElement(c => c.Marked("lblErrMessage").Text("True"));
            var result = app.Query(c => c.Marked("lblErrMessage").Text("True"));
            Assert.IsTrue(result.Any(), "DeleteKey_WithExistingKey Failed");
        }
    }
}

