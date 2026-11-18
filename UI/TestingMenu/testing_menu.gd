extends Control


func _ready() -> void:
	Selector.set_buttons([$BtnUITesting, $BtnTestLevel])


func _on_btn_ui_testing_pressed() -> void:
	Scenes.swap("UI Testing")

func _on_btn_test_level_pressed() -> void:
	Scenes.swap("Test Level")
