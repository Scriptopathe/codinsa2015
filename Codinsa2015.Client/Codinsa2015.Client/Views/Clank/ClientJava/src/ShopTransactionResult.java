public enum ShopTransactionResult
{
	ItemDoesNotExist(0),
	ItemIsNotAConsummable(1),
	NoItemToSell(2),
	NotEnoughMoney(3),
	NotInShopRange(4),
	UnavailableItem(5),
	ProvidedSlotDoesNotExist(6),
	NoSlotAvailableOnHero(7),
	EnchantForNoWeapon(8),
	StackOverflow(9),
	Success(10);
	int _value;
	ShopTransactionResult(int value) { _value = value; } 
	public int getValue() { return _value; }
	public static ShopTransactionResult fromValue(int value) { 
		ShopTransactionResult val = ItemDoesNotExist;
		val._value = value;
		return val;
	}
}
