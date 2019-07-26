function safeGet(data) {
  try {
    return JSON.parse(data);
  } catch (e) {
    return null;
  }
}

class WebStorage {
  constructor(storage) {
    this._storage = storage;
  }

  get(key) {
    return safeGet(this._storage.getItem(key));
  }

  set(key, value) {
    const valueStr = JSON.stringify(value);
    this._storage.setItem(key, valueStr);
  }

  remove(key) {
    this._storage.removeItem(key);
  }

  clear() {
    this._storage.clear()
  }
}

class MemoryStorage {
  constructor() {
    this.$dataMap = new Map();
  }
  get(key) {
    this.$dataMap.get(key);
  }

  set(key, value) {
    this.$dataMap.set(key, value);
  }

  remove(key) {
    this.$dataMap.delete(key);
  }
  clear() {
    this.$dataMap.clear()
  }
}

class CookieStorage {
  get(key) {
    var name = key + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
      var c = ca[i].trim();
      if (c.indexOf(name) == 0)
        return c.substring(name.length, c.length);
    }
    return "";
  }

  set(key, value, exdays = 1) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toGMTString();
    document.cookie = key + "=" + value + "; " + expires;
  }

  remove(key) {
    var exp = new Date();
    exp.setTime(exp.getTime() - 1);
    var cval = this.get(key);
    if (cval != null)
      document.cookie = key + "=" + cval + ";expires=" + exp.toGMTString();
  }

  clear() {
    var date = new Date();
    date.setTime(date.getTime() - 10000);
    var keys = document.cookie.match(/[^ =;]+(?=\=)/g);
    if (keys) {
      for (var i = keys.length; i--;)
        document.cookie = keys[i] + "=0; expire=" + date.toGMTString();
    }
  }
}
export const storage = {
  local: new WebStorage(localStorage),
  session: new WebStorage(sessionStorage),
  memory: new MemoryStorage(),
  cookie: new CookieStorage()
};
