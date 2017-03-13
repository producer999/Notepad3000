Notepad3000

Legend:
**** = planned feature/fix for next release/commit

Next:

-****add Font button functionality
-****fix MainTextBox resizing lag when resizing program vertically
-****figure out how to integrate scrolling (and word wrap) with the InkCanvas
-****update file save flags when Control-Z, Control-X is pressed (requires some work - override OnKeyPressed)
-****check if you want to save file if you click the application window X to exit (may not be possible)
-****fix pasting from another document different using Paste menu or Cntl-V
-****save a version of every file that is opening from disk locally as its opened to protect loss


v0.02.01 (3/12/2017)

-give focus to the RichEditBox after program start up by giving is TabIndex = 0
-add time/date to edit menu
	-add date/time functionality to Edit menu
	-create keyboard shortcut for date/time
-fix file save flags not updating when pasting text with control-v and edit->paste
-create keyboard shortcuts for close file
-create keyboard shortcut for exit program
-when Save As an existing file, put the current file name in the FilePicker suggestion
-prevent keyboard focus from leaving MainTextBox when menu is clicked
-move cursor to end of datetime after insertion so you can continue to type
-add clear to Edit menu and add functionailty
-add functionality to Edit menu (undo, cut, copy, paste) 
-add select all functionality to Edit menu
-changed Font under Format menu to Word Wrap ToggleMenuFlyoutItem with IsChecked = True
-implemented word wrap on/off with the Click event of the Format menu button and MainTextBox.TextWrapping
~~~when pasting from outside the program convert all text to the same font, size and color using Paste event
	~~~this kind of works but it behaves differently depending on whether Paste is selected from menu or Control-V is pressed
-fixed bug isCntlKeyPressed flag not setting to false after Cntl-W, Cntl-S when MessageDialog appears
-changed min build to Windows 10 Anniversary in Project-Properties to support AllowFocusOnInteraction = False


v0.01.05 (3/10/2017)

-show current open/saved filename on top bar
-add a star next to filename on top bar when file is unsaved
-fix text formatting on saving then loading the same file by using TextGetOptions.UseCrlf on save
-debug the File menu options and load/save
-create 3-way message dialog and proper checks, return Task<string> instead of Task<bool> in async
-add a check to see if you want to save after clicking Open using MessageDialog
-add a check to see if you want to save after clicking New using MessageDialog
-add a check to see if you should just save or open save file picker
-create keyboard shortcuts for save
-make it so that the file changed flags ignore when you press control or shift
-add exit menu option functionality
	-add a check to see if you want to save after exiting using MessageDialog
-add a check to see if you want to save after clicking Close using MessageDialog


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


v.01.03 (3/8/17)

-Change Tab key in RichEditBox to insert a tab instead of tabbing through controls
-Decrease BorderThickness on RichEditBox to 1
-Add MenuFlyOut to each menu button with options
	-fix positioning of menu Button Flyouts
-Connect GitHub to project
-add basic event functionality for New button


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


v.01.01

-UWP App
-use RichEditBox for textLayer
	-font = consolas, size = 14
	-turn off auto spell check

_________________________________________________________________________________________________
BUGS:


Bug: Open File -> highlight text -> Hit Control-X
	result: cut works as expected but file save flags are not updated and it thinks the file hasnt been changed
	expected: isFileSaved = false, textChanged = true
	fix suggestion: this may be calling OnKeyDown() instead of KeyDown()

Bug: Open File -> type or delete text -> Hit Control-Z
	result: undo works as expected but file save flags are not updated and it thinks the file hasnt been changed
	expected: isFileSaved = false, textChanged = true
	fix suggestion: this may be calling OnKeyDown() instead of KeyDown()

Bug: Hit Control-Q
	result: nothing happens
	expected: application checks to save file, closes file and exits application
	fix suggestion: this may be calling OnKeyDown() instead of KeyDown()

Bug: Resize application window vertically
	result: the MainTextBox resizes slowly and lags behind the grid resize
	expected: no lag when redrawing RichEditBox

_______________

Improved Bug: Repeatedly Save by pressing Ctrl-S
	result: UnauthorizedAccess exception
	expected: no exception
	improved: v0.01.05
	improved notes: repeatedly saving doesn't automatically trigger it thought it comes up sometimes randomly

_______________

Potentially Fixed Bug: Open -> select file -> change text -> Hit Control-W -> Hit Cancel
     Open -> select file -> change text -> Hit Control-S -> Hit Cancel
	result: controlPressed flag gets stuck on and pressing S or W with no shift activates the shortcuts
	result notes: this also causes additional unwanted behavior:
		-save flags dont get set correctly when typing other characters
		-it is possible to unexpectedly close an unsaved file by pressing W or Q
	expected: typing coninues normally as if control is no longer pressed
	fix suggestion: KeyUp event never fires when releasing shift after the MessageDialog has focus
	potentially fixed: v.0.02.01
	fixed:
	fix notes: manually set isCntlKeyPressed to fasle on all shortcuts that can bring up a MessageDialog

Potentially Fixed Bug: New -> type -> New -> Save -> Cancel
	result: clears text anyway
	expected: leaves text intact, textChanged = true, file = null, isCurrentFileSaved = false
	potentially fixed: v0.01.05
	fixed: 

Potentially Fixed Bug: New -> Open -> choose a file -> type text -> New -> Save
	result: exception, text clears, file saves
	expected: no exception
	info: New -> Open -> choose a file -> type text -> Open -> Save does not throw exception
	potentially fixed: v0.01.05 
	fixed:
	fix notes: SaveCurrentFile() was not awaitable and under New the CurrentFile = null call would occur before the Save was complete. Made SaveCurrentFile() retuen Task<bool> fixed it.

_______________

