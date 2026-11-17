@tool class_name UIButton extends Control


@onready var background: ColorRect = $Background
@onready var label: RichTextLabel = $Label

@export_tool_button("Refresh") var refresh := func() -> void:
	label.size = self.size
	background.size = self.size
	label.global_position = self.global_position
	label.text = text
	background.global_position = self.global_position
	background.color = color_default
	label.add_theme_color_override("default_color", color_text_default)
@export var color_default := Color.DIM_GRAY
@export var color_hover := Color.DARK_BLUE
@export var text := "Button Text"
@export var color_text_default := Color.WHITE
@export var color_text_hover := Color.ROYAL_BLUE


func _ready() -> void:
	refresh.call()

func _process(delta: float) -> void:
	if Engine.is_editor_hint(): return
	
	var color := label.get_theme_color("default_color")
	if not is_hovered:
		background.color = LerpHelper.l(background.color, color_default, 12.0, delta)
		color = LerpHelper.l(color, color_text_default, 8.0, delta)
	else:
		background.color = LerpHelper.l(background.color, color_hover, 12.0, delta)
		color = LerpHelper.l(color, color_text_hover, 8.0, delta)
	label.add_theme_color_override("default_color", color)



var is_hovered := false
signal hovered
signal unhovered
signal pressed

func hover() -> void:
	is_hovered = true
	hovered.emit()

func unhover() -> void:
	is_hovered = false
	unhovered.emit()

func press() -> void:
	pressed.emit()

func get_selector_placement() -> Vector2:
	return global_position + size + Vector2(4, size.y * -0.5)
