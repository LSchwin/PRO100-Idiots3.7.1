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

func _on_btn_sound_pressed(change := 10) -> void:
	var vol := change_volume(Persistent.get_setting("sound"), change)
	Persistent.set_setting("sound", vol)
	$BarSound.value = vol
	update_settings()

func _on_btn_sound_hovered_input(action: String, pressed: bool) -> void:
	if not pressed: return
	if action == "menu_left":
		_on_btn_sound_pressed(-10)
	elif action == "menu_right":
		_on_btn_sound_pressed(10)

func _on_btn_music_pressed(change := 10) -> void:
	var vol := change_volume(Persistent.get_setting("music"), change)
	Persistent.set_setting("music", vol)
	$BarMusic.value = vol
	update_settings()


func _on_btn_music_hovered_input(action: String, pressed: bool) -> void:
	if not pressed: return
	if action == "menu_left":
		_on_btn_music_pressed(-10)
	elif action == "menu_right":
		_on_btn_music_pressed(10)

func change_volume(current_volume := 100, amount := 10) -> int:
	current_volume += amount
	if current_volume > 100:
		current_volume = 0
	elif current_volume < 0:
		current_volume = 100
	return current_volume

func update_settings() -> void:
	Persistent.set_setting("window_mode", DisplayServer.window_get_mode())
	Persistent.update_settings()
