import React from 'react';

import {
    ActionButton, getTheme, Icon, Link, MessageBar, MessageBarType, Separator, Stack, Text
} from '@fluentui/react';
import { useBoolean } from '@fluentui/react-hooks/lib/useBoolean';

//import { AddProjectPanel } from "../components/settings/addProjectPanel";
//import { AddProject, CloseProject, GetProjects } from "../connectors";
import { GetProjects, IProject } from './project';

//import Context from "../context";

const theme = getTheme()

type IMessage = {
	content: string | JSX.Element,
	type?: MessageBarType
}

export const Settings = () => {
	const [message, setMessage] = React.useState<IMessage>()
	const [projects, setProjects] = React.useState<IProject[]>([])

	const loadProjects = () => { GetProjects().then(setProjects) }
	React.useEffect(loadProjects, [])

	const [showPanel, { toggle: toggleShowPanel }] = useBoolean(false)
	/*const onAddProject = React.useCallback(
		async (name: string) => {
			const project = await AddProject(name)
			if (project) {
				setMessage({ content: "Project added" })

				loadProjects()
				return undefined
			}
			return "Could not add project"
		}, [])*/

	//const ctx = React.useContext(Context)
	/*const onCloseProject = React.useCallback((project: IProject) => {
		if (!ctx.showDialog) return

		const onClosed = (s: boolean): void => {
			if (s) {
				setMessage({ content: "The project was closed" });
				loadProjects();
			}
			else
				setMessage({ content: "An error occurred closing the project", type: MessageBarType.error });
		};

		ctx.showDialog({
			title: "Close Project",
			content: `Are you sure you want to close the project '${project.title}'?`,
			onOkPressed: () => CloseProject(project.id).then(onClosed)
		})
	}, [ctx])*/

	return (
		<>
			{message && <MessageBar
				messageBarType={message.type ?? MessageBarType.success}
				onDismiss={() => setMessage(undefined)}>{message.content}</MessageBar>}
			<Stack>
				<Text variant="xLarge">Settings</Text>
				<Stack.Item style={{ margin: theme.spacing.m }}>
					<Text variant="large">Projects</Text>
					<Separator />
					<Stack tokens={{ childrenGap: theme.spacing.s2 }}>
						<ActionButton iconProps={{ iconName: 'Add' }}
							text="Add Project" onClick={toggleShowPanel} />
						{projects.map((p) => (
							<Stack horizontal tokens={{ childrenGap: theme.spacing.s2 }} key={p.projectId}>
								<Link title="Close Project"
									/*onClick={() => onCloseProject(p)}*/>
									<Icon iconName="Cancel" />
								</Link>
								<Link
									href={"/settings/project/" + p.projectId}>
									{p.title}
								</Link>
							</Stack>
						))}
					</Stack>
				</Stack.Item>
			</Stack>
			{/*<AddProjectPanel
				onAdd={onAddProject}
				onDismiss={toggleShowPanel}
				isOpen={showPanel} />*/}
		</>
	)
};