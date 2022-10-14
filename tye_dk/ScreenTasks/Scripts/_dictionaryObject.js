// JScript File
// Javascript Dictionary Object
// borrowed from
// 4 Guys From Rolla.com
// http://4guysfromrolla.com/webtech/100800-1.shtml

function jsDictionary() {
  this.Add = jsDictionary_mAdd;
  this.Lookup = jsDictionary_mLookup;
  this.Delete = jsDictionary_mDelete;
  this.Exists = jsDictionary_mExists;
}

 function jsDictionary_mLookup(strKeyName) {
   return(this[strKeyName]);
 }
 
 function jsDictionary_mExists(strKeyName) {
   return(typeof(this[strKeyName]) != "undefined" && this[strKeyName] != null);
 }
  
 function jsDictionary_mAdd() {
   for (c=0; c < jsDictionary_mAdd.arguments.length; c+=2) {
        this[jsDictionary_mAdd.arguments[c]] = jsDictionary_mAdd.arguments[c+1];
   }
 }

 function jsDictionary_mDelete(strKeyName) {
    for (c=0; c < jsDictionary_mDelete.arguments.length; c++) {
      this[jsDictionary_mDelete.arguments[c]] = null;
    }
  }