namespace Aspire.Hosting.AspireC4;

/// <summary>
/// Unit tests for the local CLI command builder logic.
/// </summary>
public sealed partial class AspireC4BuilderTests
{
	[Test]
	public async Task BuildLocalCLICommand_Npx_UsesNpxWithLikeC4Args()
	{
		// Arrange

		// Act
		var (command, args) = AspireC4Builder.BuildLocalCLICommand(LocalCLIRuntime.Npx, "/tmp/likec4", 5173);

		// Assert
		await Assert.That(command).IsEqualTo("npx");
		await Assert.That(args).IsEquivalentTo(["likec4", "serve", "/tmp/likec4", "--port", "5173"]);
	}

	[Test]
	public async Task BuildLocalCLICommand_Pnpm_UsesPnpmDlx()
	{
		// Arrange

		// Act
		var (command, args) = AspireC4Builder.BuildLocalCLICommand(LocalCLIRuntime.Pnpm, "/tmp/likec4", 5173);

		// Assert
		await Assert.That(command).IsEqualTo("pnpm");
		await Assert
			.That(args)
			.IsEquivalentTo(["dlx", "--ignore-workspace", "likec4", "serve", "/tmp/likec4", "--port", "5173"]);
	}

	[Test]
	public async Task BuildLocalCLICommand_Yarn_UsesYarnDlx()
	{
		// Arrange

		// Act
		var (command, args) = AspireC4Builder.BuildLocalCLICommand(LocalCLIRuntime.Yarn, "/tmp/likec4", 5173);

		// Assert
		await Assert.That(command).IsEqualTo("yarn");
		await Assert
			.That(args)
			.IsEquivalentTo([
				"dlx",
				"--package",
				"likec4",
				"--package",
				"react",
				"--package",
				"react-dom",
				"likec4",
				"serve",
				"/tmp/likec4",
				"--port",
				"5173",
			]);
	}

	[Test]
	public async Task BuildLocalCLICommand_Bun_UsesBunx()
	{
		// Arrange

		// Act
		var (command, args) = AspireC4Builder.BuildLocalCLICommand(LocalCLIRuntime.Bun, "/tmp/likec4", 5173);

		// Assert
		await Assert.That(command).IsEqualTo("bunx");
		await Assert.That(args).IsEquivalentTo(["--bun", "likec4", "serve", "/tmp/likec4", "--port", "5173"]);
	}

	[Test]
	public async Task BuildLocalCLICommand_Deno_UsesDenoRunWithNodeModulesDirNone()
	{
		// Arrange

		// Act
		var (command, args) = AspireC4Builder.BuildLocalCLICommand(LocalCLIRuntime.Deno, "/tmp/likec4", 5173);

		// Assert
		await Assert.That(command).IsEqualTo("deno");
		await Assert
			.That(args)
			.IsEquivalentTo([
				"run",
				"--allow-all",
				"--node-modules-dir=none",
				"npm:likec4",
				"serve",
				"/tmp/likec4",
				"--port",
				"5173",
			]);
	}

	[Test]
	public async Task BuildLocalCLICommand_Auto_Throws()
	{
		// Arrange

		// Act
		static (string Command, string[] Args) Action() =>
			AspireC4Builder.BuildLocalCLICommand(LocalCLIRuntime.Auto, "/tmp", 5173);

		// Assert
		await Assert.That(Action).Throws<ArgumentOutOfRangeException>();
	}

	[Test]
	public async Task BuildLocalCLICommand_IncludesCorrectPort()
	{
		// Arrange

		// Act
		var (_, args) = AspireC4Builder.BuildLocalCLICommand(LocalCLIRuntime.Npx, "/output", 9090);

		// Assert
		await Assert.That(args).Contains("9090");
	}
}
