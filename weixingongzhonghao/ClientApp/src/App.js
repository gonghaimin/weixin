import React, { Component } from 'react';
import { routes } from './routes.config';
import { Route, Switch } from 'react-router';
import zhCN from 'antd/lib/locale-provider/zh_CN'
import { LocaleProvider } from 'antd'
import { BrowserRouter } from 'react-router-dom';

export default class App extends Component {

  render() {
    return (
      <LocaleProvider locale={zhCN}>
        <BrowserRouter basename={this.props.basename}>
          <div>
            <Switch>
              {
                routes.map((route, index) => {
                  return (<Route
                    key={index}
                    path={route.path}
                    exact={route.exact}
                    render={props => {
                      return <route.component {...props} routes={[]} />;
                    }}
                  />)
                })
              }
            </Switch>
          </div>
        </BrowserRouter>
      </LocaleProvider>
    );
  }
}


