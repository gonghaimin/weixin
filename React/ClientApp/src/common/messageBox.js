import { message, Modal } from 'antd';

function noop() {}
const defaultOptions = {
  duration: 2
};

const defaultConfirmOptions = {
  title: '系统提示',
  okText: '确定',
  cancelText: '取消'
};

export const messageBox = {
  info(content, onClose = noop, options = defaultOptions) {
    message.info(content, options.duration, onClose);
  },

  success(content, onClose = noop, options = defaultOptions) {
    message.success(content, options.duration, onClose);
  },

  error(content, onClose = noop, options = defaultOptions) {
    message.error(content, options.duration, onClose);
  },

  loading(content, onClose = noop, options = defaultOptions) {
    message.loading(content, options.duration, onClose);
  },

  confirm(content, onOk = noop, onCancel = noop, options = defaultConfirmOptions) {
    const opt = Object.assign({}, options, { content, onOk, onCancel });
    return Modal.confirm(opt);
  },

  destroy() {
    message.destroy();
  }
};
