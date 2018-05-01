using System;
using System.Collections.Generic;
using NUnit.Framework;
using SecureStorageSample.PlugInServices;
using SecureStorageSample.ViewModels;
using SecureStorageSampleNUnitTests.Mocks;
using UnitTestSample.Mocks;
using Xamarin.Forms;

namespace SecureStorageSampleNUnitTests
{
    /// <summary>
    /// Unit test class
    /// </summary>
    [TestFixture]
    public class MainViewModelUnitTest
    {
        /// <summary>
        /// One time setup
        /// </summary>
        [OneTimeSetUp]
        public void InitOnce()
        {
            // init Xamarin forms mocks
            Xamarin.Forms.Mocks.MockForms.Init();
            RegisterImplementations();
        }

        /// <summary>
        /// Runs prior to each test
        /// </summary>
        [SetUp]
        public void InitTest()
        {
            var plugInProvider = DependencyService.Get<IPlugInProvider>();
            UnitTestDataStore = ((SecureStorageMock)plugInProvider.SecureStorage).MockStorage;
            // clear before each test
            UnitTestDataStore.Clear();
            _viewModel = new MainPageViewModel();
        }

        #region SetCommand UnitTests
        /// <summary>
        /// Verify that SetCommand stores data as expected.
        /// </summary>
        [Test]
        public void SetCommand_WithValidValue_StoresValue()
        {
            // Arrange
            _viewModel.SetVal = ValidTestValue;
            _viewModel.Key = ValidTestKey;

            // Act
            _viewModel.SetCommand.Execute(null);

            // Assert
            Assert.AreEqual(ValidTestValue, UnitTestDataStore[ValidTestKey]);
        }

        /// <summary>
        /// Verify that when innvalid key is used in SetCommand, error message is displayed
        /// </summary>
       [Test]
        public void SetCommand_WithInvalidKey_SetsErrMesage()
        {
            // Arrange
            _viewModel.SetVal = ValidTestValue;
            _viewModel.Key = string.Empty;

            // Act
            _viewModel.SetCommand.Execute(null);

            // Assert
            Assert.IsFalse(string.IsNullOrEmpty(_viewModel.ErrMessage));
        }
        #endregion

        #region GetCommand UnitTests
        /// <summary>
        /// Validate that the GetCommand loads valid value in the view model
        /// </summary>
       [Test]
        public void GetComand_WithValidKey_ReturnsCorrectValue()
        {
            // Arrange
            UnitTestDataStore[ValidTestKey] = ValidTestValue;
            _viewModel.Key = ValidTestKey;

            // Act
            _viewModel.GetCommand.Execute(null);

            // Assert
            Assert.AreEqual(ValidTestValue, _viewModel.GetVal);
        }

        /// <summary>
        /// Verify that when innvalid key is used in GetCommand, error message is displayed
        /// </summary>
       [Test]
        public void GetComand_WithInvalidKey_DisplaysErrorMessage()
        {
            // Arrange
            _viewModel.Key = string.Empty;

            // Act
            _viewModel.GetCommand.Execute(null);

            // Assert
            Assert.IsFalse(string.IsNullOrEmpty(_viewModel.ErrMessage));
        }
        #endregion

        #region HasCommand UnitTests
        /// <summary>
        /// Validate that the HasCommand sets "Y" in HasVal in the view model
        /// When value is present
        /// </summary>
       [Test]
        public void HasComand_WithExistingKey_ReturnsY()
        {
            // Arrange
            UnitTestDataStore[ValidTestKey] = ValidTestValue;
            _viewModel.Key = ValidTestKey;

            // Act
            _viewModel.HasCommand.Execute(null);

            // Assert
            Assert.AreEqual("Y", _viewModel.HasVal);
        }

        /// <summary>
        /// Validate that the HasCommand sets "N" in HasVal in the view model
        /// When value is present
        /// </summary>
       [Test]
        public void HasComand_WithNonExistingKey_ReturnsN()
        {
            // Arrange
            _viewModel.Key = "NotExistingKey";

            // Act
            _viewModel.HasCommand.Execute(null);

            // Assert
            Assert.AreEqual("N", _viewModel.HasVal);
        }

        /// <summary>
        /// Verify that when innvalid key is used in GetCommand, error message is displayed
        /// </summary>
       [Test]
        public void HasComand_WithInvalidKey_DisplaysErrorMessage()
        {
            // Arrange
            _viewModel.Key = string.Empty;

            // Act
            _viewModel.HasCommand.Execute(null);

            // Assert
            Assert.IsFalse(string.IsNullOrEmpty(_viewModel.ErrMessage));
        }
        #endregion

        #region DeleteCommand UnitTests
        /// <summary>
        /// Validate that the DeleteCommand sets "True" in ErrMessage in the view model
        /// When key is present
        /// </summary>
       [Test]
        public void DeleteComand_WithExistingKey_ReturnsTrue()
        {
            // Arrange
            UnitTestDataStore[ValidTestKey] = ValidTestValue;
            _viewModel.Key = ValidTestKey;

            // Act
            _viewModel.DeleteCommand.Execute(null);

            // Assert
            Assert.AreEqual(true.ToString(), _viewModel.ErrMessage);
        }

        /// <summary>
        /// Validate that the DeleteCommand sets "False" in ErrMessage in the view model
        /// When key is not present
        /// </summary>
       [Test]
        public void DeleteComand_WithNonExistingKey_ReturnsN()
        {
            // Arrange
            _viewModel.Key = "NotExistingKey";

            // Act
            _viewModel.DeleteCommand.Execute(null);

            // Assert
            Assert.AreEqual(false.ToString(), _viewModel.ErrMessage);
        }

        /// <summary>
        /// Verify that when innvalid key is used in DeleteCommand, error message is displayed
        /// </summary>
       [Test]
        public void DeleteComand_WithInvalidKey_DisplaysErrorMessage()
        {
            // Arrange
            _viewModel.Key = string.Empty;

            // Act
            _viewModel.DeleteCommand.Execute(null);

            // Assert
            Assert.IsFalse(string.IsNullOrEmpty(_viewModel.ErrMessage));
        }
        #endregion

        #region private methods
        /// <summary>
        /// register the mock provider
        /// </summary>
        private void RegisterImplementations()
        {
            DependencyService.Register<IPlugInProvider, PluginProviderMock>();
        }
        #endregion
        /// <summary>
        /// ViewModel that will be tested using mock
        /// </summary>
        private MainPageViewModel _viewModel;

        private Dictionary<string, string> UnitTestDataStore;
        // private SecureStorageMock UnitTestDataStore;

        #region consts
        const string ValidTestValue = "1234";
        const string ValidTestKey = "FooKey";
        #endregion
    }
}
