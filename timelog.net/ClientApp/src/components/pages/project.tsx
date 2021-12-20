import React from 'react';
import { Outlet, useNavigate, useParams } from 'react-router-dom';

import { CommandBar, Dialog, getTheme, Stack, Text } from '@fluentui/react';

import { TaskActions } from '../taskActions';

//import { TaskActions } from "../components/taskActions"
//import { CloseProject, GetProject, GetTasks } from "../connectors/entriesConnector"
//import { ITask } from "../../../server/tasks"
//import Context from "../context"
//import { AddTask } from "../components/project/addTask"

export type IProject = {
	projectId: string;
	title: string;
	tasks: ITask[];
}

export type ITask = {
	projectId: string;
	taskId: string;
	title: string;
	externalId?: string;
	url?: string;
	icon?: string;
	entries: IEntry[];
}

export type IEntry = {
	id: string;
	task: ITask;
	startTime: Date;
	endTime?: Date;
};

export type IItem = IEntry & {
	key: string;
	duration: number
}

const theme = getTheme()

const getCommands = (addTask: () => void, closeProject: () => void) => [
	{
		key: "addTask",
		text: "Add Task",
		iconProps: { iconName: "Add" },
		onClick: addTask
	},
	{
		key: "closeProject",
		text: "Close Project",
		iconProps: { iconName: "Cancel" },
		onClick: closeProject
	}
]

export const Project = () => {
	const { projectId, taskId } = useParams()
	const [project, setProject] = React.useState<IProject>()
	//const [tasks, setTasks] = React.useState<ITask[]>([])

	//const ctx = React.useContext(Context)

	React.useEffect(() => {
		(async () => {
			if (!projectId) return
			const project = await GetProject(projectId)
			//const tasks = await GetTasks(id)

			setProject(project)
			//setTasks(tasks)
		})()
	}, [projectId])

	const addTask = React.useCallback(() => {
		/*if (!ctx.showDialog) return

		ctx.showDialog({
			title: "Add Task",
			content: <AddTask />
		})*/
	}, [])

	const closeProject = React.useCallback(() => {
		/*if (!ctx.showDialog) return

		ctx.showDialog({
			title: "Close Project",
			content: "Are you sure you want to close the project?",
			onOkPressed: () => CloseProject(id).then()
		})*/
	}, [])

	const commands = getCommands(addTask, closeProject)
	const navigate = useNavigate()

	return (
		<Stack>
			<Text variant="xLarge">Project: {project?.title}</Text>
			<CommandBar items={commands} />
			<Text variant="large">Active Tasks</Text>
			{project && project.tasks.map((t, i) =>
				<Stack horizontal verticalAlign="center" key={"t" + i} tokens={{ childrenGap: theme.spacing.s2}}>
					{<TaskActions item={t} />}
					{t.title}
				</Stack>)}
			<Dialog hidden={!taskId} title="Edit Task" onDismiss={() => navigate("./")}>
				<Outlet />
			</Dialog>
		</Stack>
	)
}

export const GetProjects = async () => {
	const response = await fetch('/project')
	return await response.json() as IProject[]
}

export const GetProject = async (projectId: string): Promise<IProject> => {
	const response = await fetch('/project/' + projectId)
	return await response.json() as IProject
}