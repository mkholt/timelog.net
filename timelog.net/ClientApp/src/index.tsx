import 'bootstrap/dist/css/bootstrap.css'
import React from 'react'
import ReactDOM from 'react-dom'
import App from './App'
import registerServiceWorker from './registerServiceWorker'
import { BrowserRouter, Route, Routes } from "react-router-dom"
import { Dashboard } from "./components/pages/dashboard"
import { Counter } from "./components/pages/Counter"
import { FetchData } from "./components/pages/FetchData"
import { Settings } from "./components/pages/settings"
import { Project } from "./components/pages/project"

const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href')
const rootElement = document.getElementById('root')

ReactDOM.render(
	<BrowserRouter basename={baseUrl}>
		<Routes>
			<Route path="/" element={<App />}>
				<Route path="/" element={<Dashboard />} />
				<Route path="counter" element={<Counter />} />
				<Route path="fetch-data" element={<FetchData />} />
				<Route path="settings" element={<Settings />}>
					<Route path="project/:id" element={<Project />} />
				</Route>
				<Route path="stats" />
				<Route path="logout" />
			</Route>
		</Routes>
	</BrowserRouter>, rootElement)

registerServiceWorker()

