class_name UITooltip extends Node2D


static func create(text: String, _owner: UIButton) -> UITooltip:
	var tooltip: UITooltip = preload("uid://dkeubmu3ibs5n").instantiate()
	_owner.add_child(tooltip)
	tooltip.setup(text, _owner)
	return tooltip


@onready var background: ColorRect = $Background
@onready var label: RichTextLabel = $Label
var tween: Tween
var button: UIButton

func _process(_delta: float) -> void:
	_process_following()


func _process_following() -> void:
	global_position = button.get_selector_placement() + Vector2(8, 8)
	
	var screen_rect := get_viewport_rect()
	var self_rect := Rect2(global_position, label.size)
	if self_rect.end.x > screen_rect.end.x:
		position.x += screen_rect.end.x - self_rect.end.x
	elif self_rect.position.x < screen_rect.position.x:
		position.x += screen_rect.position.x - self_rect.position.x
	if self_rect.end.y > screen_rect.end.y:
		position.y += screen_rect.end.y - self_rect.end.y
	elif self_rect.position.y < screen_rect.position.y:
		position.y += screen_rect.position.y - self_rect.position.y

func setup(text: String, _owner: UIButton) -> void:
	button = _owner
	
	if tween: tween.kill()
	tween = create_tween()
	tween.tween_property(self, "modulate:a", 1.0, 0.1).from(0.0)
	
	label.text = ""
	label.reset_size()
	label.text = text
	await get_tree().process_frame
	background.size = label.size + Vector2(4,4)


func delete() -> void:
	if tween: tween.kill()
	tween = create_tween()
	tween.tween_property(self, "modulate:a", 0.0, 0.1)
	await tween.finished
	queue_free()
