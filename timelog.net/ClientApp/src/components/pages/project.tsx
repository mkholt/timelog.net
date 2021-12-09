import { CommandBar, ICommandBarItemProps, Stack, Text } from "@fluentui/react"
import React from "react"
import { useParams } from "react-router-dom"
//import { TaskActions } from "../components/taskActions"
//import { CloseProject, GetProject, GetTasks } from "../connectors/entriesConnector"
//import { ITask } from "../../../server/tasks"
//import Context from "../context"
//import { AddTask } from "../components/project/addTask"

export type IProject = {
	id: string;
	title: string;
}

export type ITask = {
	projectId: string;
	taskId: string;
	title: string;
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

type ProjectParams = {
	id: string
}

const getCommands = (addTask: () => void, closeProject: () => void): ICommandBarItemProps[] => [
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
	const { id } = useParams<ProjectParams>()
	const [project, setProject] = React.useState<IProject>()
	//const [tasks, setTasks] = React.useState<ITask[]>([])

	//const ctx = React.useContext(Context)

	/*useEffect(() => {
		(async () => {
			const project = await GetProject(id)
			const tasks = await GetTasks(id)

			setProject(project)
			setTasks(tasks)
		})()
	}, [id])*/

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
	}, [id])

	const commands = getCommands(addTask, closeProject)

	return (
		<Stack>
			<Text variant="xLarge">Project: {project?.title}</Text>
			<CommandBar items={commands} />
			<Text variant="large">Active Tasks</Text>
			{/*tasks.map((t, i) =>
				<Stack horizontal verticalAlign="center" key={"t" + i}>
					<TaskActions item={t} />
					<Text>{t.title}</Text>
				</Stack>)*/}
		</Stack>
	)
}

export const GetProjects = async () => {
	const response = await fetch('project')
	return await response.json() as IProject[]
}