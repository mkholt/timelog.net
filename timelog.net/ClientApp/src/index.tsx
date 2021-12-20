import React from 'react';
import ReactDOM from 'react-dom';
import { BrowserRouter, Outlet, Route, Routes } from 'react-router-dom';

import { initializeIcons } from '@fluentui/react';

import App from './App';
import { Dashboard } from './components/pages/dashboard';
import { Project } from './components/pages/project';
import { Settings } from './components/pages/settings';
import { Task } from './components/task';
import registerServiceWorker from './registerServiceWorker';

const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href') ?? ""
const rootElement = document.getElementById('root')

initializeIcons()

ReactDOM.render(
	<BrowserRouter basename={baseUrl}>
		<Routes>
			<Route path="/" element={<App />}>
				<Route path="/" element={<Dashboard />} />
				<Route path="settings" element={<Outlet />}>
					<Route path="" element={<Settings />} />
					<Route path="project/:projectId" element={<Project />}>
						<Route path="task/:taskId" element={<Task />} />
					</Route>
				</Route>
				<Route path="stats" />
				<Route path="logout" />
			</Route>
		</Routes>
	</BrowserRouter>, rootElement)

registerServiceWorker()

