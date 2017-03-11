Notepad3000

Legend:
**** = planned feature/fix for next release/commit

v.01.01

-UWP App
-use RichEditBox for textLayer
	-font = consolas, size = 14
	-turn off auto spell check


v.01.02 (3/7/2017)

-use StackPanel (Horizontal) for main menu
	-add File Menu Button
	-add Edit menu Button
	-add Format menu Button
	-add Help menu Button
-Add ScrollViewer on bottom
	-add RichEditBox to ScrollViewer
	-Remove Page/App size parameters
	-Bottom ScrollBar to Hidden
	-Right ScrollBar to Visible
	

v.01.03 (3/8/17)

-Change Tab key in RichEditBox to insert a tab instead of tabbing through controls
-Decrease BorderThickness on RichEditBox to 1
-Add MenuFlyOut to each menu button with options
	-fix positioning of menu Button Flyouts
-Connect GitHub to projectd
-add basic event functionality for New button


v.01.04 (3/9/17)

-add file open functionality
-add file save functionality
-add file close functionality
-add save as functionality
-added About button functionailty MessageDialog
-add a boolean to see whether the text has been changed (should we ask for open/new to save?)
	-was not working correctly when under TextChanged event so I moved it to KeyPressed event
-add a boolean to check if current file is saved
-add a check to see if you want to save after clicking New using MessageDialog
-add a check to see if you want to save aftder clicking Open using MessageDialog
-change background color of Menu Buttons
-change background color of App TitleBar
-decrease menu button padding
-use RichEditBox's ScrollView instead of using an wrapper ScrollView
	-Make it so text view scrolls automatically with ScrollViewer as you type (solved by above)
	-collapse text down to remain on screen when you resize from large to small (solved by above)
	

v0.01.05 (3/10/2017)

-****show current open/saved filename on top bar
-fix text formatting on saving then loading the same file by using TextGetOptions.UseCrlf on save
-****debug the menu options and load/save
-create 3-way message dialog and proper checks, return Task<string> instead of Task<bool> in async
-add a check to see if you want to save after clicking Open using MessageDialog
-****add a check to see if you want to save after clicking Close using MessageDialog
-add a check to see if you want to save after clicking New using MessageDialog
-****add a check to see if you want to save after exiting using MessageDialog
-add a check to see if you should just save or open save file picker
-****figure out how to integrate scrolling (and word wrap) with the InkCanvas
-****create keyboard shortcuts for save, select all, close file, close program



BUGS:

Potentially Fixed Bug: New -> type -> New -> Save -> Cancel
	result: clears text anyway
	expected: leaves text intact, textChanged = true, file = null, isCurrentFileSaved = false
	potentially fixed: v0.01.05
	fixed: 

Bug: New -> Open -> choose a file -> type text -> New -> Save
	result: exception, text clears, file saves
	expected: no exception
	info: New -> Open -> choose a file -> type text -> Open -> Save does not throw exception
