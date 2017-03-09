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
-Connect GitHub to project
-add basic event functionality for New button


Next:

-****add file open functionality
-****add file save functionality
-****add a check to see if you want to save after clicking New
-**** Change background color of Menu Buttons
-****decrease menuflyout padding??
-****Make it so text view scrolls automatically with ScrollViewer as you type
-****collapse text down to remain on screen when you resize from large to small
		-****figure out how to integrate this (and word wrap) with the InkCanvas
