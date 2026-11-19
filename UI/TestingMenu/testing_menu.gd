extends Control


func _ready() -> void:
	$Buttons.activate()


func _on_back_to_main_menu_pressed() -> void:
	Scenes.swap("Main Menu")

func _on_btn_ui_testing_pressed() -> void:
	Scenes.swap("UI Testing")

func _on_btn_test_level_pressed() -> void:
	Scenes.swap("Test Level")

func _on_btn_movement_test_room_pressed() -> void:
	Scenes.swap("Movement Test Room")
