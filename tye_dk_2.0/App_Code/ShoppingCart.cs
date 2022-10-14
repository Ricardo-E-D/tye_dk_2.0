using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tye.Data;
using System.Web.Hosting;

/// <summary>
/// Summary description for ShoppingCart
/// </summary>
public class ShoppingCart {

	Dictionary<EquipmentItem, int> items = new Dictionary<EquipmentItem, int>();

	public ShoppingCart() {
		if (HttpContext.Current == null)
			return;
		if (HttpContext.Current.Session == null) {
			return;
		}
		if (HttpContext.Current.Session[SessionKeys.ShoppingCart.ToString()] == null) {
			HttpContext.Current.Session.Add(SessionKeys.ShoppingCart.ToString(), items);
		}
		items = (Dictionary<EquipmentItem, int>)HttpContext.Current.Session[SessionKeys.ShoppingCart.ToString()];
	}

	private EquipmentItem getItem(EquipmentItem item) {
		return items.FirstOrDefault(n => n.Key.ID == item.ID).Key;
	}

	public void AddEquipment(EquipmentItem Item, int Quantity = 1) {
		var exists = getItem(Item);
		if (exists != null) {
			items[exists] = items[exists] + Quantity;
		} else {
			items.Add(Item, Quantity);
		}
		exists = getItem(Item);
		if (exists != null && items[exists] < 1)
			items.Remove(exists);
		save();
	}

	private void save() {
		HttpContext.Current.Session[SessionKeys.ShoppingCart.ToString()] = items;
	}

	public void RemoveEquipment(EquipmentItem Item) {
		var exists = getItem(Item);
		if (exists != null)
			items.Remove(exists);
		save();
	}

	public double GetCartTotalPrice(Language Lang) {
		double total = 0.0;
		foreach (var item in items.Keys) {
			var info = item.Infos.FirstOrDefault(n => n.LanguageID == Lang.ID); // equipmentitem only has one per language
			if (info != null) {
				total += (info.Price * items[item]);
			}
		}
		return total;
	}

	public int GetCartTotalItems() {
		return items.Values.Sum();
		//int total = 0;
		//foreach (var item in items.Keys) {
		//   total += (items[item]);
		//}
		//return total;
	}

	public List<object> DescribeCart(Language Lang) {
		List<object> objs = new List<object>();
		using (var ipa = statics.GetApi()) {
			foreach (var item in items.Keys) {
				var quip = ipa.EquipmentGetSingle(item.EquipmentID);

				if (quip == null)
					continue;

				var quipInfo = quip.Infos.FirstOrDefault(n => n.LanguageID == Lang.ID);
				if (quipInfo == null)
					continue;

				var info = item.Infos.FirstOrDefault(n => n.LanguageID == Lang.ID); // equipmentitem only has one per language
				if (info != null) {
					objs.Add(new {
						EquipmentItemID = item.ID,
						EquipmentName = quipInfo.Name,
						EquipmentItemDescription = info.Description,
						EquipmentPrice = info.Price,
						Quantity = items[item],
						SubTotal = items[item] * info.Price
					});
				}
			}
		}
		return objs;
	}

	public void OneLess(int EquipmentItemID) {
		var key = items.Keys.FirstOrDefault(n => n.ID == EquipmentItemID);
		if(key != null) {
			if (items[key] <= 1)
				items.Remove(key);
			else
				items[key] -= 1;
		}
		save();
	}

	public void OneMore(int EquipmentItemID) {
		var key = items.Keys.FirstOrDefault(n => n.ID == EquipmentItemID);
		if (key != null) {
			items[key] += 1;
		}
		save();
	}

	public void Clear() {
		items.Clear();
		save();
		//HttpContext.Current.Session[SessionKeys.ShoppingCart.ToString()] = null;
	}

	public void RemoveEquipment(int EquipmentItemID) {
		var key = items.Keys.FirstOrDefault(n => n.ID == EquipmentItemID);
		if (key != null) {
				items.Remove(key);
		}
		save();
	}
}