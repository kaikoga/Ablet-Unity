# Changelog

All notable changes to this project will be documented in this file.
The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [3.0.0] - 2026-05-24

### Added

- Supports Unity 6.4.
- Subplatform is introduced.
  - Added VRChat PC / Android / iOS as builtin Subplatforms of builtin VRChat Platform.
- Added Copy buttons to Error Reports.

### Changed

- API has breaking changes. Notably BuildArgument.
- Supports Nullable Reference Types.
- Supports Loch 3.0 breaking changes.

## [2.3.1] - 2026-03-05

### Fixed

- Fixed compile error without NDMF.
- Fixed compile error without Loch.

## [2.3.0] - 2026-02-26

### Added

- Added i18n support when Loch is also installed. Loch is an optional dependency for Ablet.

## [2.2.0] - 2026-02-06

### Added

- Prunes Missing scripts. (This is just after Importing Phase for now)

### Changed

- Serializing generated assets and prefab is done in the very last order.   

### Fixed

- Really fixed Manual Apply asset cleanup.

## [2.1.0] - 2026-02-01

### Added

- Added SerializedReferencePath (this is yet experimental)

### Fixed

- Fixed Inplace Preview sometimes not enabled.
- Fixed Manual Apply asset cleanup prompting too often.
- Fixed NDMF interop.

## [2.0.0] - 2026-01-17

- Initial release.
