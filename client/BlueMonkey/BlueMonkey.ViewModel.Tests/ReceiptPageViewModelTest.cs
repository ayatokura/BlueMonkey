﻿using System.ComponentModel;
using System.IO;
using System.Windows.Media;
using BlueMonkey.MediaServices;
using BlueMonkey.Model;
using BlueMonkey.ViewModels;
using Moq;
using Xunit;

namespace BlueMonkey.ViewModel.Tests
{
    public class ReceiptPageViewModelTest
    {
        [Fact]
        public void ReceiptProperty()
        {
            var editExpense = new Mock<IEditExpense>();
            var actual = new ReceiptPageViewModel(editExpense.Object);

            Assert.NotNull(actual.Receipt);
            Assert.Null(actual.Receipt.Value);

            var image = File.ReadAllBytes("lena.jpg");
            editExpense.Setup(m => m.Receipt).Returns(new MediaFile(".jpg", image));
            editExpense.Raise(m => m.PropertyChanged += null, new PropertyChangedEventArgs("Receipt"));

            Assert.NotNull(actual.Receipt.Value);
            // ImageSource doesn't expose any mechanism to retrieve the original byte array.
            // For this reason, the test will give up by ImageSource.
        }

        [Fact]
        public void PickPhotoAsyncCommand()
        {
            var editExpense = new Mock<IEditExpense>();
            editExpense.Setup(m => m.IsPickPhotoSupported).Returns(true);

            var actual = new ReceiptPageViewModel(editExpense.Object);

            Assert.NotNull(actual.PickPhotoAsyncCommand);
            Assert.True(actual.PickPhotoAsyncCommand.CanExecute());

            actual.PickPhotoAsyncCommand.Execute();

            editExpense.Verify(m => m.PickPhotoAsync(), Times.Once);
        }

        [Fact]
        public void PickPhotoAsyncCommandWhenIsPickPhotoSupportedFalse()
        {
            var editExpense = new Mock<IEditExpense>();
            editExpense.Setup(m => m.IsPickPhotoSupported).Returns(false);

            var actual = new ReceiptPageViewModel(editExpense.Object);

            Assert.NotNull(actual.PickPhotoAsyncCommand);
            Assert.False(actual.PickPhotoAsyncCommand.CanExecute());
        }

        [Fact]
        public void TakePhotoAsyncCommand()
        {
            var editExpense = new Mock<IEditExpense>();
            editExpense.Setup(m => m.IsTakePhotoSupported).Returns(true);

            var actual = new ReceiptPageViewModel(editExpense.Object);

            Assert.NotNull(actual.TakePhotoAsyncCommand);
            Assert.True(actual.TakePhotoAsyncCommand.CanExecute());

            actual.TakePhotoAsyncCommand.Execute();

            editExpense.Verify(m => m.TakePhotoAsync(), Times.Once);
        }

        [Fact]
        public void TakePhotoAsyncCommandWhenIsTakePhotoSupportedFalse()
        {
            var editExpense = new Mock<IEditExpense>();
            editExpense.Setup(m => m.IsTakePhotoSupported).Returns(false);

            var actual = new ReceiptPageViewModel(editExpense.Object);

            Assert.NotNull(actual.TakePhotoAsyncCommand);
            Assert.False(actual.TakePhotoAsyncCommand.CanExecute());
        }


        [Fact]
        public void Destroy()
        {
            var editExpense = new Mock<IEditExpense>();
            var actual = new ReceiptPageViewModel(editExpense.Object);
            actual.Destroy();
            // It is impossible to test for code review.
        }
    }
}
