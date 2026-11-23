extends Control


func _ready() -> void:
	$UIButtonSelection.activate()
	$BarSound.value = Persistent.get_setting("sound")
	$BarMusic.value = Persistent.get_setting("music")


func _on_btn_back_pressed() -> void:
	Scenes.swap("Main Menu")

func _on_btn_fullscreen_pressed() -> void:
	if DisplayServer.window_get_mode() == DisplayServer.WINDOW_MODE_FULLSCREEN:
		DisplayServer.window_set_mode(DisplayServer.WINDOW_MODE_WINDOWED)
	else:
		DisplayServer.window_set_mode(DisplayServer.WINDOW_MODE_FULLSCREEN)
	update_settings()

func _on_btn_sound_pressed() -> void:
	var vol: float = Persistent.get_setting("sound")
	if vol >= 100:
		vol = 0
	else:
		vol += 10
	Persistent.set_setting("sound", vol)
	$BarSound.value = vol
	update_settings()

func _on_btn_music_pressed() -> void:
	var vol: float = Persistent.get_setting("music")
	if vol >= 100:
		vol = 0
	else:
		vol += 10
	Persistent.set_setting("music", vol)
	$BarMusic.value = vol
	update_settings()

func update_settings() -> void:
	Persistent.set_setting("window_mode", DisplayServer.window_get_mode())
	Persistent.update_settings()
