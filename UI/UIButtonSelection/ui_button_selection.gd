class_name UIButtonSelection extends Node


@export var buttons: Array[UIButton] = []

func activate(vertical := true) -> void:
	Selector.set_buttons(buttons as Array, vertical)
