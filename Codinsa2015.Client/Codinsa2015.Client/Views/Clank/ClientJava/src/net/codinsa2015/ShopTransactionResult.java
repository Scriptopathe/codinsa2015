package net.codinsa2015;
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
	Success(10),
	AlreadyMaxLevel(11);
	int _value;
	ShopTransactionResult(int value) { _value = value; } 
	public int getValue() { return _value; }
	public static ShopTransactionResult fromValue(int value) { 
		switch(value) { 
			case 0: return ItemDoesNotExist;
			case 1: return ItemIsNotAConsummable;
			case 2: return NoItemToSell;
			case 3: return NotEnoughMoney;
			case 4: return NotInShopRange;
			case 5: return UnavailableItem;
			case 6: return ProvidedSlotDoesNotExist;
			case 7: return NoSlotAvailableOnHero;
			case 8: return EnchantForNoWeapon;
			case 9: return StackOverflow;
			case 10: return Success;
			case 11: return AlreadyMaxLevel;
		}
		return ItemDoesNotExist;
	}
}
