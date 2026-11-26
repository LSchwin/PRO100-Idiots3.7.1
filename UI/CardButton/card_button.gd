@tool class_name CardButton extends UIButton


@export var card: CardResource

@onready var icon: Sprite2D = $Icon
@onready var description_label: RichTextLabel = $DescriptionLabel
@onready var name_label: RichTextLabel = $NameLabel


var target_position := Vector2.ZERO
var movement_tween: Tween
var holder: Node2D


func _ready() -> void:
	var color_change := randf_range(-0.1, 0.0)
	color_default.h += color_change
	color_hover.h += color_change
	super._ready()

func _process(delta: float) -> void:
	super._process(delta)
	if not is_hovered and (!movement_tween or not movement_tween.is_running()):
		movement_tween = null
		var pos := target_position
		if holder: pos += holder.global_position
		global_position = LerpHelper.lv2(self.global_position, pos, 16.0, delta)

func refresh() -> void:
	super.refresh()
	if not Engine.is_editor_hint() and not card: 
		push_error(str(get_path()) + " does not have a card attached!")
		return
	if Engine.is_editor_hint():
		if card:
			card = null
			push_warning("Do not attach a card to this scene!")
		return
	
	name_label.text = card.Name
	icon.texture = card.Texture
	description_label.text = card.Description
	background.size -= Vector2(2,2)
	background.position += Vector2(1,1)
	if card.Action == null: $Border.color = Color.BLACK
	else:                   $Border.color = Color.WHITE

signal pressed_card(card: CardButton)
func press() -> void:
	super.press()
	pressed_card.emit(self)

var scale_tween: Tween
func hover() -> void:
	super.hover()
	z_index = 1
	if scale_tween: scale_tween.kill()
	scale_tween = create_tween()
	scale_tween.tween_property(self, "scale", Vector2(DeckLayer.CARDS_HOVERED_SCALE,DeckLayer.CARDS_HOVERED_SCALE), 0.2)

func unhover() -> void:
	super.unhover()
	z_index = 0
	if scale_tween: scale_tween.kill()
	scale_tween = create_tween()
	scale_tween.tween_property(self, "scale", Vector2(DeckLayer.CARDS_UNHOVERED_SCALE,DeckLayer.CARDS_UNHOVERED_SCALE), 0.2)
