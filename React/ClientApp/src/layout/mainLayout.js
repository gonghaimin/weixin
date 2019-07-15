import 'antd/dist/antd.css';
import './mainLayout.css';
import { Layout, Menu, Icon, message } from 'antd';
import React from 'react';
import { Link } from 'react-router-dom'; // 路由\
import { menus } from './menus'
const { Header, Sider } = Layout;


export default class MainLayout extends React.Component {
    state = {
        collapsed: false,
        selectedKeys: ["11"]
    };

    toggle = () => {
        this.setState({
            collapsed: !this.state.collapsed,
        });
    };

    getNavMenuItems = (menusData) => {
        if (!menusData) {
            return [];
        }
        return menusData.map(item => {
            if (!item.name) {
                return null;
            }
            if (item.children && item.children.some(child => child.name)) {
                return (
                    <Menu.SubMenu
                        title={
                            <span>
                                <Icon type={item.icon} />
                                <span>{item.name}</span>
                            </span>
                        }
                        key={item.key || item.path}
                    >
                        {this.getNavMenuItems(item.children, '')}
                    </Menu.SubMenu>
                );
            }
            const icon = item.icon && <Icon type={item.icon} />;
            return (
                <Menu.Item key={item.key || item.path} onClick={() => {
                    this.setState({ selectedKeys: [item.key] })
                    message.success(item.key)
                }} >
                    <Link to={item.path} replace={item.path === window.location.pathname}>
                        {icon}
                        <span>{item.name}</span>
                    </Link>
                </Menu.Item>
            );
        });
    }

    render() {
        return (
            <Layout>
                <Sider trigger={null} collapsible collapsed={this.state.collapsed} style={{
                    overflow: 'auto',
                    height: '100vh',
                    left: 0,
                }}>
                    <div className="logo" style={{ color: '#eae1e1' }}>微信公众号平台接口开发</div>
                    <Menu
                        theme="dark"
                        mode="inline"
                        selectedKeys={this.state.selectedKeys}
                        defaultOpenKeys={['1']}
                    >
                        {this.getNavMenuItems(menus)}
                    </Menu>
                </Sider>
                <Layout>
                    <Header style={{ background: '#fff', padding: 0 }}>
                        <Icon
                            className="trigger"
                            type={this.state.collapsed ? 'menu-unfold' : 'menu-fold'}
                            onClick={this.toggle}
                        />
                    </Header>
                    {this.props.children}
                </Layout>
            </Layout>
        );
    }
}


