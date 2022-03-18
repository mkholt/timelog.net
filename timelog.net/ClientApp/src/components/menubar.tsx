import React from 'react';
import { useNavigate } from 'react-router-dom';

import { INavLink, Nav } from '@fluentui/react';

export const Menubar = () => {
	const navigate = useNavigate()
	return (
		<Nav
			onLinkClick={(event: React.MouseEvent|undefined, element: INavLink | undefined) => {
				event?.preventDefault()
				navigate(element?.url ?? "/")
			}}
			groups={links}
		/>
	)
}

export const toKey = (n: string, prefix: string) => prefix + "-" + n.toLowerCase().replace(/\s+/g, '-')

const link = (name: string, url: string, iconName: string) => ({
	name: name,
	key: toKey(name, "nav"),
	url: url,
	iconProps: {
		iconName: iconName
	}
})

const links = [
	{
		links: [
			link('Dashboard', '/', 'News'),
			link('Settings', '/settings', 'PlayerSettings'),
			link('Stats', '/stats', 'StackedLineChart'),
		]
	},
	{
		links: [
			link('Log out', '/logout', 'Signout'),
		]
	}
]