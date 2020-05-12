<template>
  <v-card>
    <v-card-title>{{ action }} Contact</v-card-title>
    <v-form @submit.prevent="saveContact()" class="pa-2">
      <v-text-field v-for="f in fields" :key="f.field" :label="f.label" v-model="contact[f.field]" :rules="f.rules" />
        {{ result }}
    </v-form>
    <v-card-actions>
      <v-spacer />
      <v-btn text type="submit" color="primary" @click="saveContact()">Save</v-btn>
    </v-card-actions>
  </v-card>
</template>
<script>
import { computed, reactive, ref, watch } from '@vue/composition-api'
import { wrapStore } from '@/lib/utils'

function fields () {
  const field = (field, label, rules = []) => ({ field, label, rules })
  const required = v => !!v || 'Required'
  
  return {
    fields: [
      field('firstName', 'First Name', [required]),
      field('lastName', 'Last Name', [required]),
      field('street', 'Street'),
      field('city', 'City'),
      field('state', 'State'),
      field('zip', 'ZIP'),
      field('homePhone', 'Home Phone'),
      field('mobilePhone', 'Mobile Phone'),
      field('workPhone', 'Work Phone'),
      field('email', 'Email')
    ]
  }
}

export default {
  props: ['id'],
  setup (props, context) {
    const { $store } = context.root
    const Contact = wrapStore($store, 'addressbook', 'contactId')
    const contact = reactive({
      firstName: '',
      lastName: '',
      street: '',
      city: '',
      state: '',
      zip: '',
      homePhone: '',
      mobilePhone: '',
      workPhone: '',
      email: ''
    })
    const result = ref('')
    const action = computed(() => props.id ? 'Edit' : 'New')
    watch(() => props.id, async id => {
      // reset contact
      for (const k in contact) {
        contact[k] = '' 
      }
      if (id === 0) {
        return
      }
      console.log(id) // eslint-disable-line no-console
      const data = await Contact.get(id)
      for (const [k,v] of Object.entries(data)) {
        contact[k] = v
      }
      console.log(data, contact) // eslint-disable-line no-console
    })
    const saveContact = async () => {
      result.value = contact
      const fn = props.id ? Contact.update : Contact.create
      try {
        const res = await fn(contact)
        context.emit('saved', res.data)
      } catch (e) {
        result.value = e
      }
    }
    return {
      ...fields(),
      action,
      contact,
      result,
      saveContact
    }
  }
}
</script>