extends Node


func _ready() -> void:
	load_settings()


#region Settings
const SETTINGS_PATH := "user://settings.cardistry"
var settings: Dictionary = {
	"window_mode": DisplayServer.WINDOW_MODE_WINDOWED,
	"sound": 100,
	"music": 100,
}

func save_settings(path := SETTINGS_PATH) -> void:
	settings.set("window_mode", DisplayServer.window_get_mode())
	
	var save_file := FileAccess.open(path, FileAccess.WRITE)
	var json_string := JSON.stringify(settings)
	save_file.store_line(json_string)
	save_file.close()

func load_settings(path := SETTINGS_PATH) -> void:
	var save_file := FileAccess.open(path, FileAccess.READ)
	if !save_file: 
		save_settings(path)
		return
	var json_string := save_file.get_line()
	var data: Variant = JSON.parse_string(json_string)
	if data is not Dictionary:
		return
	settings = data
	save_file.close()
	
	update_settings()

func update_settings() -> void:
	AudioServer.set_bus_volume_linear(1, settings.get("music") * 0.01)
	AudioServer.set_bus_volume_linear(2, settings.get("sound") * 0.01)
	DisplayServer.window_set_mode(get_setting("window_mode"), 0)

func get_setting(setting: String) -> Variant:
	return settings.get(setting, null)

func set_setting(setting: String, value: Variant) -> void:
	settings.set(setting, value)
#endregion settings

func _notification(what: int) -> void:
	if what == NOTIFICATION_WM_CLOSE_REQUEST:
		save_settings()
