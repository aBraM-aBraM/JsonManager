# JsonManager
A tool I've created to manage a json database easily

The setup is quite easy.
First run the program once, a 'db_config.cfg' file hopefully emerged :)
In this file you can configure what kind of format your jsons will be built in

The configuration is quite simple just an array of strings for each attribute you want each
item of the database to have.

The default configuration is ["name,"type"] meaning the json is built as followed
{
"0":{"name":"my_name","type":"my_type"},
"1":{"name":"other_name","type":"other_type"}
}

The program is quite intuitive to work with
you can add items modify and remove.

Useful Keys!
ESC - go back to the main menu
TAB - reverse the order of the items menu (for bigger databases ease of use)
ENTER - accept
ARROW KEYS - general manouvering

hopefully you find it helpful, Eyal
