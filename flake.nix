{
  description = "A fresh retake of the React API in Fable, optimized for happiness.";

  inputs = {
    nixpkgs.url = "github:NixOS/nixpkgs/nixos-unstable";
    flake-parts = {
      url = "github:hercules-ci/flake-parts";
      inputs.nixpkgs-lib.follows = "nixpkgs";
    };
    allSystems.url = "github:nix-systems/default";
  };

  outputs =
    inputs@{ self, flake-parts, ... }:
    flake-parts.lib.mkFlake { inherit inputs; } {
      systems = import inputs.allSystems;

      perSystem =
        {
          self',
          lib,
          pkgs,
          system,
          ...
        }:
        let
          permittedPackages = [
            "dotnet-core-combined"
            "dotnet-wrapped-combined"
            "dotnet-combined"
            "dotnet-sdk-6.0.428"
            "dotnet-sdk-wrapped-6.0.428"
            "dotnet-sdk-6.0.136"
          ];
          libPath =
            with pkgs;
            lib.makeLibraryPath [
              glib
              nss
              nspr
              atk
              cups
              libdrm
              dbus
              expat
              xorg.libX11
              xorg.libXcomposite
              xorg.libXdamage
              xorg.libXext
              xorg.libXfixes
              xorg.libXrandr
              xorg.libxcb
              xorg.libxshmfence
              libxkbcommon
              libxcomp
              libgbm
              gtk3
              pango
              cairo
              alsa-lib
            ];
        in
        {
          _module.args.pkgs = import self.inputs.nixpkgs {
            inherit system;
            config = {
              permittedInsecurePackages = permittedPackages;
            };
          };
          devShells.default = pkgs.mkShell {
            inputsFrom = builtins.attrValues self'.packages;
            env = {
              # Will work as soon as PuppeteerSharp is updated on Fable Mocha
              # Both are defined at https://pptr.dev/api/puppeteer.chromesettings
              PUPPETEER_CHROME_SKIP_DOWNLOAD = true;
              PUPPETEER_SKIP_CHROME_DOWNLOAD = true;
              PUPPETEER_SKIP_DOWNLOAD = true;
              NODE_OPTIONS = "--openssl-legacy-provider";
              PUPPETEER_EXECUTABLE_PATH = "${pkgs.chromium}/bin/chromium";
              PUPPETEER_BROWSER = "chrome";
            };
            packages = with pkgs; [
              (
                with dotnetCorePackages;
                combinePackages [
                  dotnetCorePackages.sdk_6_0
                  dotnetCorePackages.sdk_8_0
                ]
              )
              fantomas
              chromium
            ];
            LD_LIBRARY_PATH = libPath;
          };
        };
    };
}
