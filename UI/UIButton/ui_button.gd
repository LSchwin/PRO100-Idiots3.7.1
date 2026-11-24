@tool class_name UIButton extends Control


@onready var background: ColorRect = $Background
@onready var label: RichTextLabel = $Label

@export_tool_button("Refresh") var refresh_button := refresh
@export var text := "Button Text"
@export_group("Colors")
@export var color_default := Color(0.25, 0.25, 0.25, 1.0)
@export var color_hover := Color.GOLDENROD
@export var color_text_default := Color.WHITE
@export var color_text_hover := Color.BLACK
@export_group("Tooltips")
@export var tooltip_text_ := ""


func _ready() -> void:
	refresh()

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

func _input(event: InputEvent) -> void:
	if not is_hovered: return
	for action in InputMap.get_actions():
		if action.left(2) == "ui": continue
		if event.is_action(action): 
			if event.is_pressed():
				hovered_input.emit(str(action), true)
			elif event.is_released():
				hovered_input.emit(str(action), false)
	

func _on_resized() -> void:
	if not Engine.is_editor_hint(): return
	
	refresh()

func refresh() -> void:
	label.size = self.size
	background.size = self.size
	label.global_position = self.global_position
	label.text = text
	background.global_position = self.global_position
	background.color = color_default
	label.add_theme_color_override("default_color", color_text_default)


var is_hovered := false
signal hovered
signal unhovered
signal pressed
signal hovered_input(action: String, pressed: bool)

func hover() -> void:
	is_hovered = true
	hovered.emit()
	
	if tooltip_text_ != "" and not tooltip:
		tooltip = UITooltip.create(tooltip_text_, self)

func unhover() -> void:
	is_hovered = false
	unhovered.emit()
	
	if tooltip:
		tooltip.delete()
		tooltip = null

func press() -> void:
	pressed.emit()
	var flash_color := Color.WHITE * 5.0
	var bg_flash_color := Color.BLACK * 5.0
	bg_flash_color.a = 1.0
	label.add_theme_color_override("default_color", flash_color)
	background.color = bg_flash_color

func get_selector_placement() -> Vector2:
	return global_position + size + Vector2(4, size.y * -0.5)


var tooltip: UITooltip = null
