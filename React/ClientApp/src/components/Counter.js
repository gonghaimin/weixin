import React, { Component } from 'react';
import { Layout } from 'antd';
import MainLayout from './../layout/mainLayout'

const { Content } = Layout;

export class Counter extends Component {
  displayName = Counter.name

  constructor(props) {
    super(props);
    this.state = { currentCount: 0 };
    this.incrementCounter = this.incrementCounter.bind(this);
  }

  incrementCounter() {
    this.setState({
      currentCount: this.state.currentCount + 1
    });
  }

  render() {
    return (
      <MainLayout>
        <Content
          style={{
            margin: '24px 16px',
            padding: 24,
            background: '#fff',
            minHeight: 280,
          }}
        >
          <div>
            <h1>Counter</h1>
            <p>This is a simple example of a React component.</p>
            <p>Current count: <strong>{this.state.currentCount}</strong></p>
            <button onClick={this.incrementCounter}>Increment</button>
          </div>
        </Content>
      </MainLayout>
    );
  }
}
