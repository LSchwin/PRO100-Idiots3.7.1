extends CanvasLayer


@onready var sprite: AnimatedSprite2D = $Sprite

var current: int = 0
var buttons: Array = []
var vertical := true


func _process(delta: float) -> void:
	if buttons.size() <= 0:
		hide()
	else:
		show()
		_process_selecting(delta)

func _process_selecting(delta: float) -> void:
	var prev := current
	var current_button: UIButton = buttons[current]
	var target_position := current_button.get_selector_placement()
	sprite.global_position = LerpHelper.lv2(sprite.global_position, target_position, 24.0, delta)
	
	# Moving selector
	var up := "menu_up"
	var down := "menu_down"
	if not vertical:
		up = "menu_left"
		down = "menu_right"
	if Input.is_action_just_pressed(up):
		current -= 1
	elif Input.is_action_just_pressed(down):
		current += 1
	if current < 0:                 current = buttons.size()-1
	elif current >= buttons.size(): current = 0
	
	# Hovering
	if current != prev:
		buttons[prev].unhover()
		buttons[current].hover()
	elif not current_button.is_hovered:
		current_button.hover()
	
	# Pressing
	if Input.is_action_just_pressed("menu_press"):
		current_button.press()
		sprite.global_position = target_position - Vector2(8, 0)


func set_buttons(b: Array, v := true) -> void:
	buttons = b
	vertical = v

func disable() -> void:
	buttons = []
