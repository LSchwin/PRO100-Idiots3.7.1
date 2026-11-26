class_name DeckLayer extends CanvasLayer


const CARDS_PER_ROW := 11
const CARDS_UNHOVERED_SCALE := 0.5
const CARDS_HOVERED_SCALE := 1.0
@onready var card_spawn_point: Node2D = $CanvasGroup/CardSpawnPoint
@onready var deck_anchor: Node2D = $CanvasGroup/DeckHolder/DeckAnchor
@onready var hand_holder: Node2D = $CanvasGroup/HandHolder

func _ready() -> void:
	for child in self.get_children():
		if child is Node2D or Control:
			$CanvasGroup.self_modulate.a = 0.0
	
	# debug
	await Delays.wait(0.5)
	const TEST_CARD_0 := preload("uid://cibfkthvglfax")
	const TEST_CARD_1 := preload("uid://d1hdj7c0phhqv")
	for i in range(10):
		if randi_range(0, 1) == 1: GameCS.AddToDeck(TEST_CARD_0)
		else:                      GameCS.AddToDeck(TEST_CARD_1)
		i += 1
	GameCS.MoveToHand(preload("uid://kxen5jasll2d"))
	GameCS.MoveToHand(TEST_CARD_0)
	open()

func _process(delta: float) -> void:
	deck_anchor.position.y = LerpHelper.lf(deck_anchor.position.y, -current_row * 32.0, 16.0, delta)
	
	if selected_card: return
	if Input.is_action_just_pressed("menu_up"):
		var row := current_row - 1
		if row < 0:
			row = deck_buttons.size()-1
		select_row(row)
	elif Input.is_action_just_pressed("menu_down"):
		var row := current_row + 1
		if row >= deck_buttons.size():
			row = 0
		select_row(row)


var fade_tween: Tween
func open() -> void:
	if fade_tween: fade_tween.kill()
	fade_tween = create_tween()
	fade_tween.tween_property($CanvasGroup, "self_modulate:a", 1.0, 0.333)
	await spawn_cards(GameCS.deck, GameCS.hand)
	select_row()

func close() -> void:
	if fade_tween: fade_tween.kill()
	fade_tween = create_tween()
	fade_tween.tween_property($CanvasGroup, "self_modulate:a", 0.0, 0.333)
	await fade_tween.finished
	queue_free()


#region Spawning & Placing
func spawn_cards(deck: Array, hand: Array) -> Signal:
	var i := 0
	var x := 0
	var y := 0
	for card in deck:
		# Setup
		var card_button: CardButton = preload("uid://dhfrl882fskvf").instantiate()
		card_button.card = card
		card_button.scale *= 0.5
		card_button.z_index = deck.size() - i
		deck_anchor.add_child(card_button)
		card_button.name = "CB_Deck_"+card.Name+"0"
		# Signals
		setup_deck_card(card_button)
		card_button.hovered_self.connect(card_hovered)
		# Placement
		card_button.global_position = card_spawn_point.global_position - card_button.size / 2.0
		var target_position := Vector2(32, 69)
		if x >= CARDS_PER_ROW:
			deck_buttons.append([])
			x = 0
			y += 1
		target_position += Vector2(x * 64, y * 32)
		place_card(card_button, target_position, i * 0.01, 0.2)
		x += 1
		i += 1
		deck_buttons[y].append(card_button)
	x = 0
	for card in hand:
		var card_button: CardButton = preload("uid://dhfrl882fskvf").instantiate()
		card_button.card = card
		card_button.scale *= 0.5
		hand_holder.add_child(card_button)
		var target_position := hand_holder.global_position + Vector2(x * 200, 0) + card_button.get_minimum_size() / -2.0
		card_button.target_position = target_position
		card_button.global_position = target_position
		card_button.name = "CB_Hand_"+card.Name+"0"
		setup_hand_card(card_button)
		card_button.hovered_self.connect(card_hovered)
		hand_buttons.append(card_button)
		x += 1
	return Delays.wait(i*0.01 + 0.2)

func setup_deck_card(card: CardButton) -> void:
	if card.pressed_card.is_connected(swap_card): card.pressed_card.disconnect(swap_card)
	card.pressed_card.connect(card_selected)
	card.holder = deck_anchor

func setup_hand_card(card: CardButton) -> void:
	if card.pressed_card.is_connected(card_selected): card.pressed_card.disconnect(card_selected)
	card.pressed_card.connect(swap_card)
	card.holder = null

func place_card(card_button: CardButton, target_position: Vector2, delay := 0.0, time := 0.2) -> void:
	await Delays.wait(delay)
	card_button.target_position = target_position
	if card_button.movement_tween: card_button.movement_tween.kill()
	card_button.movement_tween = create_tween()
	card_button.movement_tween.tween_property(card_button, "global_position", target_position, time).set_trans(Tween.TRANS_EXPO).set_ease(Tween.EASE_OUT)
	await card_button.movement_tween.finished
	card_button.z_index = 0
#endregion spawning & placing


#region Selecting & Swapping
var hovered_card: CardButton
var current_row := 0
var deck_buttons: Array[Array] = [[]]
var hand_buttons: Array
var selected_card: CardButton

func select_row(row := current_row) -> void:
	current_row = row
	var current_column: int = 0
	for column in deck_buttons:
		if column.has(hovered_card):
			current_column = column.find(hovered_card)
	if current_column >= deck_buttons[current_row].size(): current_column = deck_buttons[current_row].size()-1
	Selector.set_buttons(deck_buttons[current_row], false, current_column)

func card_hovered(button: UIButton) -> void:
	hovered_card = button

func card_selected(card: CardButton) -> void:
	if card.card.Action == null: return
	selected_card = card
	Selector.set_buttons(hand_buttons, false)

func swap_card(active: CardButton, idle: CardButton = selected_card) -> void:
	if GameCS.hand.has(idle.card):
		select_row()
		selected_card = null
		return
	
	GameCS.SwapCard(idle.card, active.card)
	Selector.disable()
	selected_card = null
	
	# Bullshit
	idle.unhover()
	active.unhover()
	var idle_index := idle.get_index()
	var active_index := active.get_index()
	idle.reparent(hand_holder)
	hand_holder.move_child(idle, active_index)
	
	# Positioning
	var idle_position := idle.target_position 
	var active_position := active.target_position
	idle.target_position = active_position
	active.target_position = idle_position
	
	# Array
	deck_buttons[current_row][deck_buttons[current_row].find(idle)] = active
	hand_buttons[hand_buttons.find(active)] = idle
	
	# Signal adjustmnet
	setup_deck_card(active)
	setup_hand_card(idle)
	
	await Delays.wait(0.2)
	active.reparent(deck_anchor)
	deck_anchor.move_child(active, idle_index)
	select_row()
#endregion selecting & swapping
