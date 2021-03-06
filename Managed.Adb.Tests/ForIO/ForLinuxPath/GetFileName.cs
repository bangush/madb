﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Camalot.Common.Extensions;
using Managed.Adb.IO;
using Ploeh.AutoFixture;
using Xunit;

namespace Managed.Adb.Tests.ForIOLinuxPath {
	public class GetFileName {

		[Fact]
		public void WhenPathContainsInvalidCharacter_ShouldThrowArgumentException ( ) {
			var fixture = new Fixture ( );
			int errorCount = 0;
			for ( var x = 0; x < LinuxPathConsts.InvalidPathChars.Length; ++x ) {
				try {
					string result = LinuxPath.GetFileName ( "/some/path/{0}-{1}.ext".With ( LinuxPathConsts.InvalidPathChars[x], fixture.Create ( "file-" ) ) );
				} catch ( ArgumentException ) {
					errorCount++;
				}
			}

			Assert.Equal ( LinuxPathConsts.InvalidPathChars.Length, errorCount );
		}

		[Fact]
		public void WhenPathIsNull_ShouldReturnNull ( ) {
			var fixture = new Fixture ( );
			var result = LinuxPath.GetFileName ( null );
			Assert.Null ( result );
		}

		[Fact]
		public void WhenPathIsDirectory_ShouldReturnEmpty ( ) {
			var fixture = new Fixture ( );
			var p1 = fixture.Create ( "/path1-" );
			var p2 = "{0}/".With ( fixture.Create ( "path2-" ) );
			var p = LinuxPath.Combine ( p1, p2 );
			var result = LinuxPath.GetFileName ( p );
			Assert.Equal ( string.Empty, result );
		}

		[Fact]
		public void WhenPathIsFileWithExtension_ShouldReturnFileWithExtension ( ) {
			var fixture = new Fixture ( );
			var p1 = fixture.Create ( "/path1-" );
			var p2 = "{0}/".With ( fixture.Create ( "path2-" ) );
			var f = "{0}{1}".With ( fixture.Create ( "file-" ), ".ext1" );
			var p = LinuxPath.Combine ( p1, p2, f );
			var result = LinuxPath.GetFileName ( p );
			Assert.Equal ( f, result );
		}

		[Fact]
		public void WhenPathIsFileWithoutExtension_ShouldReturnFileWithoutExtension ( ) {
			var fixture = new Fixture ( );
			var p1 = fixture.Create ( "/path1-" );
			var p2 = "{0}/".With ( fixture.Create ( "path2-" ) );
			var f = "{0}".With ( fixture.Create ( "file-" ) );
			var p = LinuxPath.Combine ( p1, p2, f );
			var result = LinuxPath.GetFileName ( p );
			Assert.Equal ( f, result );
		}
	}
}

