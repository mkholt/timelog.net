import React from 'react';
import { useNavigate, useParams } from 'react-router-dom';

import { DefaultButton, getTheme, PrimaryButton, Stack, TextField } from '@fluentui/react';

import { IconPicker } from './IconPicker';
import { ITask } from './pages/project';

const theme = getTheme()

export const Task = () => {
	const { taskId } = useParams()

	const [task, setTask] = React.useState<ITask>()
	const [newTask, setNewTask] = React.useState<INewTask>({})
	const navigate = useNavigate()

	React.useEffect(() => { if (taskId) { loadTask(taskId).then(t => setTask(t)) } }, [taskId])
	
	const onClose = React.useCallback(() => navigate("../"), [navigate])
	const onSave = React.useCallback(async () => {
		if (!task) return;
		if (await saveTask(newTask, task)) onClose()
	}, [onClose, task, newTask])

	return (
		<Stack tokens={{ childrenGap: theme.spacing.m }}>
			<Stack tokens={{ childrenGap: theme.spacing.s2 }}>
				<TextField label={"Title"} value={newTask.title ?? task?.title} onChange={(_, t) => setNewTask({...newTask, title: t})} />
				<TextField label={"External ID"} value={newTask.externalId ?? task?.externalId} onChange={(_, i) => setNewTask({...newTask, externalId: i})} />
				<TextField label={"URL"} value={newTask.url ?? task?.url} onChange={(_, u) => setNewTask({...newTask, url: u})} />
				<IconPicker label={"Icon"} value={newTask.icon ?? task?.icon} onChange={i => setNewTask({...newTask, icon: i})} />
			</Stack>
			<Stack horizontal tokens={{ childrenGap: theme.spacing.s2 }} horizontalAlign={"end"}>
				<PrimaryButton text={"Save"} onClick={onSave} />
				<DefaultButton text={"Cancel"} onClick={onClose} />
			</Stack>
		</Stack>
	)
}

const loadTask = async (taskId: string) => {
	const resp = await fetch("/Task/" + taskId)
	return await resp.json() as ITask
}

type INewTask = Partial<Pick<ITask,'title'|'externalId'|'url'|'icon'>>
const saveTask = async (task: INewTask, prevTask: ITask) => {
	const body: Partial<ITask> = {}
	
	let k: keyof INewTask
	for (k in task) {
		const move = prevTask[k] !== task[k]
		if (move) {
			body[k] = task[k]
		}
	}
	
	console.table([prevTask, task, body])
	
	const resp = await fetch("/Task/" + prevTask.taskId, {
		method: "PATCH",
		headers: {
			'Content-Type': "application/json"
		},
		body: JSON.stringify(body)
	})
	
	if (resp.ok) {
		// TODO: Show message somehow
		return true
	}
	else {
		// TODO: Show error somehow
		return false
	}
}