import React, { Component } from 'react';
import { Route } from 'react-router-dom';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { FetchData } from './components/FetchData';
import { Counter } from './components/Counter';
import EnhancedTable from './components/SmsInbox'
import './custom.css'
import ContactAdd from './components/contact/ContactAdd';
import ContactList from './components/contact/ContactList';
import SendSmsAdd from './components/sms/SendSmsAdd';
import { ToastProvider } from "react-toast-notifications"
import SendSmsItems from './components/sms/SendSmsList';
import DailyReportOpening from './components/DailyReport/DailyReportOpening';
import DailyReportClosing from './components/DailyReport/DailyReportClosing';
import UssdAdd from './components/ussd/Ussd';
import PortSetting from './components/portSetting/PortSetting';
import Login from './components/Auth/Login';
import Register from './components/Auth/Register';
import GroupAdd from './components/group/groupAdd';
import GroupList from './components/group/groupList';
import GroupWiseSms from './components/groupsmssend/GroupWiseSms';

export default class App extends Component {
  static displayName = App.name;
   
  render () {
      return (
        <ToastProvider>
      <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/counter' component={Counter} />
        <Route path='/fetch-data' component={FetchData} />
        <Route path='/smsInbox' component={EnhancedTable} />
        <Route path='/contactAdd' component={ContactAdd} />
        <Route path='/contactList' component={ContactList} />
        <Route path="/sendSms" component={SendSmsAdd} />
        <Route path="/sendSmsList" component={SendSmsItems} />
        <Route path="/dailyReportOpening" component={DailyReportOpening} />
        <Route path="/dailyReportClosing" component={DailyReportClosing} />
        <Route path="/ussdAdd" component={UssdAdd}/>
        <Route path="/portSetting" component={PortSetting} />
        <Route path="/login" component={Login} />
        <Route path="/register" component={Register} />
        <Route path="/groupAdd" component={GroupAdd} />
                  <Route path="/groupList" component={GroupList} />
                  <Route path="/groupWiseSms" component={GroupWiseSms} />

        </Layout>
              </ToastProvider>
    );
  }
}
