# Changelog
# All notable changes to this project will be documented in this file.
* Adding asset info via context menu
* Adding asset category change via context menu

## v1.4.6.3
* Fixed a bug when mods_subscriber folder do not exit or is empty
* Improved handling of empty mods
 
## v1.4.6.2
* Fixed a bug preventing loading "Surfaces" category folders in the Category Selector
* Added more tooltips

## v1.4.6.1
* Fixed window updating after creating local asset or deleting local asset
* Added Change Category option in the context menu for local assets

## v1.4.5.3
* Added automatic icon creation for missin icon.png files
* Fixed a bug that causing Surface folder not being read in by Mod Mode

## v1.4.5.2
* Improved context menu for assets (icons and separators)

## v1.4.5.1
* Added bulk operations for enabling and disabling assets
* Added bulk operations for deleting local assets (To recycle bin)
* Added bulk operations to set Draw Order, UiPriority and Decal Layer Mask for local assets
* When hovering over an asset the InfoBar will show UiPriority, Draw Order and Decal Layer Mask values
 
## v1.4.4.1
* Changed versioning scheme (major.minor.revision.build)
* Now the program create a setting file to store the menu mode
* Added a button to select EAI assets in the by mod menu
* Changed "EAI Sorting" check label to "EAI MODE"
* Fixed opening asset location when spurious symbols are used in folder asset name

## v1.4.3 r3
* Fixed surfaces being always placed in the "Surfaces" folder in EAI custom assets when EAI Sorting mode was enabled
* Added small overlay icon to local versions of assets when EAI Sorting mode is enabled (this don't overwrite the original icon)
* Fixed main window losing focus after deleting an asset
* Added EAI mod attribution in "About" window


## v1.4.3 r2
* Using EAI default settings values if an error is produced when validating a property in asset editor (if 0 or empty, if not, will apply the maximum value)
* Added new custom message window after creating local copy of assets =)
* Removed unnecessary message after canceling the creation of a local asset copy
* After a successful save in the asset editor, the Asset Editor window should close after the "Save" message.

## v1.4.3 r1
* Adding asset category change via context menu
* Fixed json reading broken json files again
* Set EAI default values if not present in the json file

## v1.4.2 r1 
* Added decal resize by percentage option in Asset Editor
* Changed the way the JSON files are read and saved in the Asset Editor
* Added About screen with Discord link

## v1.4.1 r3
* Working EAI Sorting mode. Enable it checking the "EAI Sorting" option (This option will be remembered in the future)
* Re worked enabling and disabling assets buttons based on selected assets.
* No more check boxes in the item list. Select the assets like any icon in File Explorer then Disable or Enable the assets.
* Fixed broken json after saving a surface asset in the editor.
* Improved asset loading in the list.

## v1.4.1 r1
* Modified selection of assets
* Fixed enabled and disabled assets mixed in list

## v1.4.0 r3
* Added ""loading"" indicator

## v1.4.0 r2
* Fixed invalid json when saving invalid loaded json on editor

## V1.4.0
* Added Custom Assets Installer

## v1.3.9
* Added button icons
* Added more mod info in the InfoBar
* Fixed InfoBar hanging on resize

## v1.3.8
* Changed UiPriority and Draw Order controls

## v1.3.7
* Added aspect ratio lock for decal size
* Added UV Scale calculation for surfaces
* Asset name can be changed in the Assets Editor

## v1.3
* Added custom assets editor

## v1.2.4
* Added info bar with total assets count (enabled and disabled) per mod and it size in Mb
* Added About information

## v1.2.3
* Added change log file
* Fixed showing unused filter option in the filter menu
* Added custom asset copy creation, deletion and renaming