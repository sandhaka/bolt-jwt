import * as React from 'react';
import Header from "./header/header";
import SideBar from "./sideBar/sideBar";
import Content from "./content/content";

class App extends React.Component {
  render() {
    return (
        <div>
            <Header/>
            <SideBar/>
            <Content/>
        </div>
    );
  }
}

export default App;
