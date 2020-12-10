﻿using System.Globalization;
using Xamarin.CommunityToolkit.Converters;
using NUnit.Framework;

namespace Xamarin.CommunityToolkit.UnitTests.Converters
{
	public class EqualConverter_Tests
	{
		public const string TestValue = nameof(TestValue);

		[Theory]
		[TestCase(200, 200)]
		[TestCase(TestValue, TestValue)]
		public void IsEqual(object value, object comparedValue)
		{
			var equalConverter = new EqualConverter();

			var result = equalConverter.Convert(value, typeof(EqualConverter_Tests), comparedValue, CultureInfo.CurrentCulture);

			Assert.IsInstanceOf<bool>(result);
			Assert.True((bool)result);
		}

		[Theory]
		[TestCase(200, 400)]
		[TestCase(TestValue, "")]
		public void IsNotEqual(object value, object comparedValue)
		{
			var equalConverter = new EqualConverter();

			var result = equalConverter.Convert(value, typeof(EqualConverter_Tests), comparedValue, CultureInfo.CurrentCulture);

			Assert.IsInstanceOf<bool>(result);
			Assert.False((bool)result);
		}
	}
}