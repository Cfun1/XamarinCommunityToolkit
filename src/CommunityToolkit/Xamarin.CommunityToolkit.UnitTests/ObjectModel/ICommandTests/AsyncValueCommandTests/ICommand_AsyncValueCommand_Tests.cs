﻿using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Exceptions;
using Xamarin.CommunityToolkit.ObjectModel;
using NUnit.Framework;

namespace Xamarin.CommunityToolkit.UnitTests.ObjectModel.ICommandTests.AsyncValueCommandTests
{
	public class ICommand_AsyncValueCommandTests : BaseAsyncValueCommandTests
	{
		[Theory]
		[TestCase(500)]
		[TestCase(0)]
		public async Task ICommand_Execute_IntParameter_Test(int parameter)
		{
			// Arrange
			ICommand command = new AsyncValueCommand<int>(IntParameterTask);

			// Act
			command.Execute(parameter);
			await NoParameterTask();

			// Assert
		}

		[Theory]
		[TestCase("Hello")]
		[TestCase(default)]
		public async Task ICommand_Execute_StringParameter_Test(string parameter)
		{
			// Arrange
			ICommand command = new AsyncValueCommand<string>(StringParameterTask);

			// Act
			command.Execute(parameter);
			await NoParameterTask();

			// Assert
		}

		[Test]
		public async Task ICommand_Execute_InvalidValueTypeParameter_Test()
		{
			// Arrange
			InvalidCommandParameterException actualInvalidCommandParameterException = null;
			var expectedInvalidCommandParameterException = new InvalidCommandParameterException(typeof(string), typeof(int));

			ICommand command = new AsyncValueCommand<string>(StringParameterTask);

			// Act
			try
			{
				command.Execute(Delay);
				await NoParameterTask();
				await NoParameterTask();
			}
			catch (InvalidCommandParameterException e)
			{
				actualInvalidCommandParameterException = e;
			}

			// Assert
			Assert.NotNull(actualInvalidCommandParameterException);
			Assert.AreEqual(expectedInvalidCommandParameterException.Message, actualInvalidCommandParameterException?.Message);
		}

		[Test]
		public async Task ICommand_Execute_InvalidReferenceTypeParameter_Test()
		{
			// Arrange
			InvalidCommandParameterException actualInvalidCommandParameterException = null;
			var expectedInvalidCommandParameterException = new InvalidCommandParameterException(typeof(int), typeof(string));

			ICommand command = new AsyncValueCommand<int>(IntParameterTask);

			// Act
			try
			{
				command.Execute("Hello World");
				await NoParameterTask();
				await NoParameterTask();
			}
			catch (InvalidCommandParameterException e)
			{
				actualInvalidCommandParameterException = e;
			}

			// Assert
			Assert.NotNull(actualInvalidCommandParameterException);
			Assert.AreEqual(expectedInvalidCommandParameterException.Message, actualInvalidCommandParameterException?.Message);
		}

		[Test]
		public async Task ICommand_Execute_ValueTypeParameter_Test()
		{
			// Arrange
			InvalidCommandParameterException actualInvalidCommandParameterException = null;
			var expectedInvalidCommandParameterException = new InvalidCommandParameterException(typeof(int));

			ICommand command = new AsyncValueCommand<int>(IntParameterTask);

			// Act
			try
			{
				command.Execute(null);
				await NoParameterTask();
				await NoParameterTask();
			}
			catch (InvalidCommandParameterException e)
			{
				actualInvalidCommandParameterException = e;
			}

			// Assert
			Assert.NotNull(actualInvalidCommandParameterException);
			Assert.AreEqual(expectedInvalidCommandParameterException.Message, actualInvalidCommandParameterException?.Message);
		}

		[Test]
		public void ICommand_Parameter_CanExecuteTrue_Test()
		{
			// Arrange
			ICommand command = new AsyncValueCommand<int>(IntParameterTask, CanExecuteTrue);

			// Act

			// Assert
			Assert.True(command.CanExecute(null));
		}

		[Test]
		public void ICommand_Parameter_CanExecuteFalse_Test()
		{
			// Arrange
			ICommand command = new AsyncValueCommand<int>(IntParameterTask, CanExecuteFalse);

			// Act

			// Assert
			Assert.False(command.CanExecute(null));
		}

		[Test]
		public void ICommand_NoParameter_CanExecuteFalse_Test()
		{
			// Arrange
			ICommand command = new AsyncValueCommand(NoParameterTask, CanExecuteFalse);

			// Act

			// Assert
			Assert.False(command.CanExecute(null));
		}

		[Test]
		public void ICommand_Parameter_CanExecuteDynamic_Test()
		{
			// Arrange
			ICommand command = new AsyncValueCommand<int>(IntParameterTask, CanExecuteDynamic);

			// Act

			// Assert
			Assert.True(command.CanExecute(true));
			Assert.False(command.CanExecute(false));
		}

		[Test]
		public void ICommand_Parameter_CanExecuteChanged_Test()
		{
			// Arrange
			ICommand command = new AsyncValueCommand<int>(IntParameterTask, CanExecuteDynamic);

			// Act

			// Assert
			Assert.True(command.CanExecute(true));
			Assert.False(command.CanExecute(false));
		}

		[Test]
		public async Task ICommand_Parameter_CanExecuteChanged_AllowsMultipleExecutions_Test()
		{
			// Arrange
			var canExecuteChangedCount = 0;

			ICommand command = new AsyncValueCommand<int>(IntParameterTask);
			command.CanExecuteChanged += handleCanExecuteChanged;

			void handleCanExecuteChanged(object sender, EventArgs e) => canExecuteChangedCount++;

			// Act
			command.Execute(Delay);

			// Assert
			Assert.True(command.CanExecute(null));

			// Act
			await IntParameterTask(Delay);

			// Assert
			Assert.True(command.CanExecute(null));
			Assert.AreEqual(0, canExecuteChangedCount);
		}

		[Test]
		public async Task ICommand_Parameter_CanExecuteChanged_DoesNotAllowMultipleExecutions_Test()
		{
			// Arrange
			var canExecuteChangedCount = 0;

			ICommand command = new AsyncValueCommand<int>(IntParameterTask, allowsMultipleExecutions: false);
			command.CanExecuteChanged += handleCanExecuteChanged;

			void handleCanExecuteChanged(object sender, EventArgs e) => canExecuteChangedCount++;

			// Act
			command.Execute(Delay);

			// Assert
			Assert.False(command.CanExecute(null));

			// Act
			await IntParameterTask(Delay);
			await IntParameterTask(Delay);

			// Assert
			Assert.True(command.CanExecute(null));
			Assert.AreEqual(2, canExecuteChangedCount);
		}

		[Test]
		public async Task ICommand_NoParameter_CanExecuteChanged_AllowsMultipleExecutions_Test()
		{
			// Arrange
			var canExecuteChangedCount = 0;

			ICommand command = new AsyncValueCommand(() => IntParameterTask(Delay));
			command.CanExecuteChanged += handleCanExecuteChanged;

			void handleCanExecuteChanged(object sender, EventArgs e) => canExecuteChangedCount++;

			// Act
			command.Execute(null);

			// Assert
			Assert.True(command.CanExecute(null));

			// Act
			await IntParameterTask(Delay);
			await IntParameterTask(Delay);

			// Assert
			Assert.True(command.CanExecute(null));
			Assert.AreEqual(0, canExecuteChangedCount);
		}

		[Test]
		public async Task ICommand_NoParameter_CanExecuteChanged_DoesNotAllowMultipleExecutions_Test()
		{
			// Arrange
			var canExecuteChangedCount = 0;

			ICommand command = new AsyncValueCommand(() => IntParameterTask(Delay), allowsMultipleExecutions: false);
			command.CanExecuteChanged += handleCanExecuteChanged;

			void handleCanExecuteChanged(object sender, EventArgs e) => canExecuteChangedCount++;

			// Act
			command.Execute(null);

			// Assert
			Assert.False(command.CanExecute(null));

			// Act
			await IntParameterTask(Delay);
			await IntParameterTask(Delay);

			// Assert
			Assert.True(command.CanExecute(null));
			Assert.AreEqual(2, canExecuteChangedCount);
		}
	}
}