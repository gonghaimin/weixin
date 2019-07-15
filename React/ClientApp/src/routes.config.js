import React from 'react';
import { Redirect } from 'react-router-dom';
import { Home } from './components/Home';
import { Counter } from './components/Counter';
import { FetchData } from './components/FetchData';


const routes = [
    {
        path: '/',
        component: Home,
        exact: true
    },
    {
        path: '/counter',
        component: Counter
    },
    {
        path: '/fetchdata/:startDateIndex?',
        component: FetchData
    },
    { path: '*', component: () => <Redirect to="/" /> },

];

export { routes };
