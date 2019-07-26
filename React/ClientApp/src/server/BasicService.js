import axios from 'axios';
import { config } from '../config';
import { messageBox, storage } from '../common';

const globalOptions = {
  headers: {
    'Content-Type': 'application/json',
    'Accept': 'application/json',
    'X-Requested-With': 'XMLHttpRequest'
  },
  service: null
};

axios.interceptors.request.use(function (config) {
  return config;
}, function (error) {
  return Promise.reject(error);
});

axios.interceptors.response.use(function (response) {
  return response;
}, function (error) {
  return Promise.reject(error);
});



export class BaseService {
  constructor(baseURL = '') {
    this.baseURL = baseURL;
  }

  get(url, options) {
    return this._request('get', url, null, options);
  }

  post(url, data, options) {
    return this._request('post', url, data, options);
  }

  put(url, data, options) {
    return this._request('put', url, data, options);
  }

  delete(url, options) {
    return this._request('delete', url, null, options);
  }

  set user(userInfo) {
    storage.local.set('cached_user_info', userInfo);
  }
  get user() {
    let user = storage.local.get('cached_user_info')
    return user
  }

  _request(method, url, data, options = {}) {
    globalOptions.service = this
    const headers = Object.assign({}, globalOptions.headers, options.headers);
    const opt = {
      baseURL: config.apiHost,
      withCredentials: true,
      method,
      url,
      data,
      params: options.params || {},
      headers
    };

    return axios(opt)
      .then(res => {
        return res;
      })
      .catch(err => {
        const { response } = err;
        if (response) {
          messageBox.error(response.statusText + ' ' + response.status);
          return Promise.reject(response);
        }
        return Promise.reject(err.message);
      });
  }
}